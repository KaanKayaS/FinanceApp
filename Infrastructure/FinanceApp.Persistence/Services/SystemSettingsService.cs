using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache memoryCache;
        private const string AI_STATUS_CACHE_KEY = "ai_status_enabled";

        public SystemSettingsService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            this.unitOfWork = unitOfWork;
            this.memoryCache = memoryCache;
        }

        public async Task<bool> IsAIEnabledAsync()
        {
            try
            {
                var setting = await unitOfWork.GetReadRepository<SystemSettings>()
                    .GetAllAsync(x => x.Key == "AI_ENABLED" && !x.IsDeleted);

                if (setting.Any())
                {
                    return bool.TryParse(setting.First().Value, out bool result) && result;
                }

                // Eğer ayar yoksa varsayılan olarak true döndür
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI durumu kontrol edilirken hata oluştu: {ex.Message}");
                // Hata durumunda varsayılan olarak false döndür (güvenli taraf)
                return false;
            }
        }

        public async Task SetAIStatusAsync(bool isEnabled)
        {
            try
            {
                var readRepo = unitOfWork.GetReadRepository<SystemSettings>();
                var writeRepo = unitOfWork.GetWriteRepository<SystemSettings>();

                var existingSetting = await readRepo.GetAsync(x => x.Key == "AI_ENABLED" && !x.IsDeleted);

                if (existingSetting != null)
                {
                    existingSetting.Value = isEnabled.ToString();
                    await writeRepo.UpdateAsync(existingSetting);
                }
                else
                {
                    var newSetting = new SystemSettings
                    {
                        Key = "AI_ENABLED",
                        Value = isEnabled.ToString(),
                        Description = "Yapay zeka servisinin aktif/pasif durumunu kontrol eder",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(3)
                    };
                    await writeRepo.AddAsync(newSetting);
                }

                await unitOfWork.SaveAsync();
                
                // AI durumu güncellendiğinde cache'i temizle
                memoryCache.Remove(AI_STATUS_CACHE_KEY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI durumu güncellenirken hata oluştu: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetSettingValueAsync(string key)
        {
            try
            {
                var setting = await unitOfWork.GetReadRepository<SystemSettings>()
                    .GetAllAsync(x => x.Key == key && !x.IsDeleted && x.IsActive);

                return setting.Any() ? setting.First().Value : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayar değeri alınırken hata oluştu ({key}): {ex.Message}");
                return null;
            }
        }

        public async Task SetSettingValueAsync(string key, string value, string description = null)
        {
            try
            {
                var readRepo = unitOfWork.GetReadRepository<SystemSettings>();
                var writeRepo = unitOfWork.GetWriteRepository<SystemSettings>();

                var existingSetting = await readRepo.GetAllAsync(x => x.Key == key && !x.IsDeleted);

                if (existingSetting.Any())
                {
                    var setting = existingSetting.First();
                    setting.Value = value;
                    if (!string.IsNullOrEmpty(description))
                        setting.Description = description;
                    await writeRepo.UpdateAsync(setting);
                }
                else
                {
                    var newSetting = new SystemSettings
                    {
                        Key = key,
                        Value = value,
                        Description = description ?? $"{key} system setting",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(3)
                    };
                    await writeRepo.AddAsync(newSetting);
                }

                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ayar değeri güncellenirken hata oluştu ({key}): {ex.Message}");
                throw;
            }
        }

        public async Task InitializeDefaultSettingsAsync()
        {
            try
            {
                var readRepo = unitOfWork.GetReadRepository<SystemSettings>();
                var writeRepo = unitOfWork.GetWriteRepository<SystemSettings>();

                // AI_ENABLED ayarı kontrolü
                var aiEnabledSetting = await readRepo.GetAllAsync(x => x.Key == "AI_ENABLED" && !x.IsDeleted);
                if (!aiEnabledSetting.Any())
                {
                    var defaultAISetting = new SystemSettings
                    {
                        Key = "AI_ENABLED",
                        Value = "true",
                        Description = "Yapay zeka servisinin aktif/pasif durumunu kontrol eder",
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow.AddHours(3)
                    };
                    await writeRepo.AddAsync(defaultAISetting);
                }

                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Varsayılan ayarlar oluşturulurken hata oluştu: {ex.Message}");
                throw;
            }
        }
    }
}
