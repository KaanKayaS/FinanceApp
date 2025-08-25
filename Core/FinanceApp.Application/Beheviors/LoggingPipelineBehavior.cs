using FinanceApp.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Beheviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger;

        public LoggingPipelineBehavior(
            IServiceScopeFactory serviceScopeFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private string MaskSensitiveData(object request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            });

            // Örnek: password, token, creditCard gibi alanları maskele
            var sensitiveKeys = new[] { "password", "pass", "token", "creditcard", "cvv" };

            foreach (var key in sensitiveKeys)
            {
                json = System.Text.RegularExpressions.Regex.Replace(
                    json,
                    $"(\"{key}\")\\s*:\\s*\".*?\"",
                    $"\"$1\":\"*****\"",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            return json;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();   // performans ölçümünü başlat
            var actionName = typeof(TRequest).Name;


            var requestData = MaskSensitiveData(request);


            // User bilgilerini al
            var httpContext = httpContextAccessor.HttpContext;
            var userIdClaim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = httpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
            var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();
            var userAgent = httpContext?.Request?.Headers["User-Agent"].ToString();

            int? userId = null;
            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            TResponse response = default;
            bool success = false;
            string? errorMessage = null;
            string? responseData = null;

            try
            {
                // İsteği işle
                response = await next();  //Bu satır, asıl command/query handler’ı çağırır.
                success = true;
                
                // Response bilgisi
                if (response != null)
                {
                    var responseType = typeof(TResponse).Name;
                    responseData = $"{{\"ResponseType\":\"{responseType}\",\"HasData\":true,\"Timestamp\":\"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\"}}";
                }
                else
                {
                    responseData = $"{{\"ResponseType\":\"{typeof(TResponse).Name}\",\"HasData\":false,\"Timestamp\":\"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\"}}";
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = $"{ex.GetType().Name}: {ex.Message}";
                logger.LogError(ex, "Error occurred while processing {ActionName} for user {UserId}", actionName, userId);
                throw; // Exception'ı yeniden fırlat
            }
            finally
            {
                stopwatch.Stop();
                
                // Audit log'u kaydet - bu işlem asenkron olarak background'da çalışsın
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // Background task için yeni scope oluştur
                        await using var scope = serviceScopeFactory.CreateAsyncScope();
                        var auditLogService = scope.ServiceProvider.GetRequiredService<IAuditLogService>();
                        
                        await auditLogService.LogActionAsync(
                            userId: userId,
                            userName: userName,
                            actionName: actionName,
                            requestData: requestData,
                            responseData: responseData,
                            durationMs: (int)stopwatch.ElapsedMilliseconds,
                            success: success,
                            errorMessage: errorMessage,
                            ipAddress: ipAddress,
                            userAgent: userAgent
                        );
                    }
                    catch (Exception ex)
                    {
                        // Audit log kaydederken hata oluşursa ana işlemi etkilemesin
                        logger.LogError(ex, "Failed to log audit information for action {ActionName}", actionName);
                    }
                }, CancellationToken.None); // Ana request'in cancel token'ını kullanmayalım

                // Performans için kontrol 
                if (stopwatch.ElapsedMilliseconds > 5000) // 5 saniyeden uzun sürerse uyar
                {
                    logger.LogWarning("Slow operation detected: {ActionName} took {ElapsedMs}ms for user {UserId}", 
                        actionName, stopwatch.ElapsedMilliseconds, userId);
                }
            }

            return response;
        }
    }
} 