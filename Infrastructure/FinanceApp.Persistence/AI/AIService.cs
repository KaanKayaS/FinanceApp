using FinanceApp.Application.Hubs;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Persistence.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.AI
{
    public class AIService(IHubContext<AIHub> hubContext, IChatCompletionService chatCompletionService, Kernel kernel, ISystemSettingsService systemSettingsService, IMemoryCache memoryCache)
    {
        private const string AI_STATUS_CACHE_KEY = "ai_status_enabled";
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5); // 5 dakika cache süresi
        public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!)
        {
            try
            {
                // AI servisi aktif mi kontrol et (cache'li)
                var isAIEnabled = await GetCachedAIStatusAsync();
                if (!isAIEnabled)
                {
                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", "Yapay zeka servisi şu anda devre dışı. Lütfen daha sonra tekrar deneyiniz.");
                    await hubContext.Clients.Client(connectionId).SendAsync("MessageComplete");
                    return;
                }

                var userInfo = AIHub.GetUserInfo(connectionId);
                var userId = userInfo?.UserId;
                var token = userInfo?.Token;

                if (string.IsNullOrEmpty(userId))
                {
                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", "Kullanıcı kimliği bulunamadı. Lütfen tekrar giriş yapın.");
                    await hubContext.Clients.Client(connectionId).SendAsync("MessageComplete");
                    return;
                }

                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
                {
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                };

                var history = HistoryService.GetChatHistory(userId);

                history.AddSystemMessage($"Kullanıcı ID: {userId}, Token: {token}. Eğer 'get_most_recent_instruction_title' fonksiyonunu çağırman gerekirse bu token'ı kullan.");
                history.AddUserMessage(prompt);
                string responseContent = "";

                Console.WriteLine($"Starting chat completion for user: {userId}, connection: {connectionId}");
                Console.WriteLine($"Prompt: {prompt}");
                Console.WriteLine($"History count: {history.Count}");

                await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: kernel))
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", response.ToString());
                    responseContent += response.ToString();
                }
                
                history.AddAssistantMessage(responseContent);
                Console.WriteLine($"Chat completion completed successfully for user: {userId}, connection: {connectionId}");
                
                // Mesaj tamamlandığında sinyal gönder
                await hubContext.Clients.Client(connectionId).SendAsync("MessageComplete");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AIService Error for connection {connectionId}:");
                Console.WriteLine($"Error Type: {ex.GetType().Name}");
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                
                // Hata mesajını client'a gönder
                await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", $"Hata oluştu: {ex.Message}");
                
                // Hata durumunda da mesajı tamamla
                await hubContext.Clients.Client(connectionId).SendAsync("MessageComplete");
                throw; // Hatayı yeniden fırlat
            }
        }

        public void CleanupUserHistory(string connectionId)
        {
            var userId = AIHub.GetUserId(connectionId);
            if (!string.IsNullOrEmpty(userId))
            {
                HistoryService.RemoveChatHistory(userId);
                Console.WriteLine($"Cleaned up chat history for user: {userId}");
            }
        }

        /// AI durumunu cache'den alır, gerekirse yeniler
        private async Task<bool> GetCachedAIStatusAsync()
        {
            // Cache'den değeri al
            if (memoryCache.TryGetValue(AI_STATUS_CACHE_KEY, out bool cachedValue))
            {
                return cachedValue;
            }

            // Cache miss - veritabanından al ve cache'e kaydet
            try
            {
                var isEnabled = await systemSettingsService.IsAIEnabledAsync();
                
                // Cache'e kaydet
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpiration,
                    Priority = CacheItemPriority.Normal
                };
                
                memoryCache.Set(AI_STATUS_CACHE_KEY, isEnabled, cacheEntryOptions);
                
                return isEnabled;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI durumu kontrol edilirken hata oluştu: {ex.Message}");
                // Hata durumunda güvenli tarafta kal (AI'yı devre dışı kabul et)
                return false;
            }
        }

        /// AI durumu cache'ini temizler (sistemden çağrılabilir)
        public void InvalidateAIStatusCache()
        {
            memoryCache.Remove(AI_STATUS_CACHE_KEY);
            Console.WriteLine("AI durumu cache'i temizlendi");
        }
    }
}


