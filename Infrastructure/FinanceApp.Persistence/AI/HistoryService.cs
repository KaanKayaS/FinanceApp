using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.AI
{
    public class HistoryService
    {
        private static readonly ConcurrentDictionary<string, ChatHistory> _chatHistories = new();

        public static ChatHistory GetChatHistory(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // Eğer userId boşsa, geçici bir chat history döndür
                return new ChatHistory();
            }

            return _chatHistories.GetOrAdd(userId, _ => new ChatHistory());
        }

        public static void RemoveChatHistory(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _chatHistories.TryRemove(userId, out _);
            }
        }

        public static void ClearAllHistories()
        {
            _chatHistories.Clear();
        }

        public static int GetActiveHistoryCount()
        {
            return _chatHistories.Count;
        }

        public static IEnumerable<string> GetActiveUserIds()
        {
            return _chatHistories.Keys.ToList();
        }
    }
}
