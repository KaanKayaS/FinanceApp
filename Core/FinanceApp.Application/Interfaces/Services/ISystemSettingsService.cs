using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface ISystemSettingsService
    {
        Task<bool> IsAIEnabledAsync();
        Task SetAIStatusAsync(bool isEnabled);
        Task<string> GetSettingValueAsync(string key);
        Task SetSettingValueAsync(string key, string value, string description = null);
        Task InitializeDefaultSettingsAsync();
    }
}
