using FinanceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar görebilir
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            this.auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAuditLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Geçersiz kullanıcı ID");
            }

            var logs = await auditLogService.GetAuditLogsByUserAsync(userId, pageNumber, pageSize);
            return Ok(new
            {
                UserId = userId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Logs = logs.Select(log => new
                {
                    log.Id,
                    log.ActionName,
                    log.ExecutedAt,
                    log.DurationMs,
                    log.Success,
                    log.ErrorMessage,
                    log.IpAddress,
                    // Hassas verileri gizle
                    HasRequestData = !string.IsNullOrEmpty(log.RequestData),
                    HasResponseData = !string.IsNullOrEmpty(log.ResponseData)
                })
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogsByAction([FromQuery] string actionName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                return BadRequest("Action name gerekli");
            }

            var logs = await auditLogService.GetAuditLogsByActionAsync(actionName, pageNumber, pageSize);
            return Ok(new
            {
                ActionName = actionName,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Logs = logs.Select(log => new
                {
                    log.Id,
                    log.UserId,
                    log.UserName,
                    log.ExecutedAt,
                    log.DurationMs,
                    log.Success,
                    log.ErrorMessage,
                    PerformanceStatus = log.DurationMs switch
                    {
                        < 100 => "Excellent",
                        < 500 => "Good", 
                        < 1000 => "Fair",
                        < 3000 => "Slow",
                        _ => "Very Slow"
                    }
                })
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetFailedActions([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var failedLogs = await auditLogService.GetFailedActionsAsync(from, to);
            return Ok(new
            {
                From = from,
                To = to,
                TotalFailedActions = failedLogs.Count(),
                Logs = failedLogs.Select(log => new
                {
                    log.Id,
                    log.UserId,
                    log.UserName,
                    log.ActionName,
                    log.ExecutedAt,
                    log.DurationMs,
                    log.ErrorMessage,
                    log.IpAddress
                })
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPerformanceStats([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var allLogs = await auditLogService.GetFailedActionsAsync(from, to);
            
            // Bu metodun tüm logları alması için service'i güncellemek gerekir
            // Şimdilik failed actions'ı kullanıyoruz, ileride tüm loglar için ayrı metod eklenebilir
            
            var stats = new
            {
                From = from,
                To = to,
                Message = "Performance istatistikleri için service'de GetAllAuditLogsAsync metodu eklenmesi gerekiyor"
            };

            return Ok(stats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditLogDetails(int id)
        {
            var log = await auditLogService.GetAuditLogByIdAsync(id);
            if (log == null)
            {
                return NotFound("Audit log bulunamadı");
            }

            // Sadece kendi loglarını görebilir
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int currentUserId) && log.UserId != currentUserId)
            {
                return Forbid("Bu audit log'u görme yetkiniz yok");
            }

            return Ok(new
            {
                log.Id,
                log.ActionName,
                log.RequestData,
                log.ResponseData,
                log.ExecutedAt,
                log.DurationMs,
                log.Success,
                log.ErrorMessage,
                log.IpAddress,
                log.UserAgent,
                PerformanceAnalysis = new
                {
                    Status = log.DurationMs switch
                    {
                        < 100 => "Excellent - Very Fast",
                        < 500 => "Good - Fast",
                        < 1000 => "Fair - Acceptable",
                        < 3000 => "Slow - Needs Optimization",
                        _ => "Very Slow - Critical Performance Issue"
                    },
                    DurationMs = log.DurationMs,
                    IsSlowQuery = log.DurationMs > 1000
                }
            });
        }
    }
} 