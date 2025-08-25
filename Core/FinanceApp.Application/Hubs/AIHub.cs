using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FinanceApp.Application.Hubs
{
    public class UserConnectionInfo
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ConnectedAt { get; set; }
    }

    public class AIHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserConnectionInfo> ConnectionTokens = new();

        public override Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext()?.Request.Query["access_token"];
            var userId = GetUserIdFromToken(token);
            
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
            {
                ConnectionTokens[Context.ConnectionId] = new UserConnectionInfo
                {
                    Token = token!,
                    UserId = userId,
                    ConnectedAt = DateTime.UtcNow
                };
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userInfo = GetUserInfo(Context.ConnectionId);
            if (userInfo != null)
            {
                Console.WriteLine($"User disconnected: {userInfo.UserId}");
            }
            
            ConnectionTokens.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        private string GetUserIdFromToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return string.Empty;

            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                return userIdClaim?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static UserConnectionInfo? GetUserInfo(string connectionId)
        {
            ConnectionTokens.TryGetValue(connectionId, out var userInfo);
            return userInfo;
        }

        public static string? GetUserId(string connectionId)
        {
            return GetUserInfo(connectionId)?.UserId;
        }

        public static string? GetToken(string connectionId)
        {
            return GetUserInfo(connectionId)?.Token;
        }

        public static void CleanupUserData(string userId)
        {
            // Belirli bir kullanıcının tüm bağlantılarını temizle
            var connectionsToRemove = ConnectionTokens
                .Where(kvp => kvp.Value.UserId == userId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var connectionId in connectionsToRemove)
            {
                ConnectionTokens.TryRemove(connectionId, out _);
            }
            
            Console.WriteLine($"Cleaned up all connections for user: {userId}, removed {connectionsToRemove.Count} connections");
        }

        public static int GetActiveConnectionCount()
        {
            return ConnectionTokens.Count;
        }

        public static IEnumerable<string> GetActiveUserIds()
        {
            return ConnectionTokens.Values.Select(v => v.UserId).Distinct();
        }
    }
}
