using FinanceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar erişebilir
    public class AdminController : ControllerBase
    {
        private readonly ISystemSettingsService systemSettingsService;

        public AdminController(ISystemSettingsService systemSettingsService)
        {
            this.systemSettingsService = systemSettingsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAIStatus()
        {
            try
            {
                var isEnabled = await systemSettingsService.IsAIEnabledAsync();
                return Ok(new
                {
                    Success = true,
                    IsAIEnabled = isEnabled,
                    Message = isEnabled ? "AI servisi aktif" : "AI servisi pasif"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "AI durumu alınırken hata oluştu",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetAIStatus([FromBody] SetAIStatusRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Geçersiz istek"
                    });
                }

                await systemSettingsService.SetAIStatusAsync(request.IsEnabled);

                return Ok(new
                {
                    Success = true,
                    Message = request.IsEnabled ? "AI servisi başarıyla açıldı" : "AI servisi başarıyla kapatıldı",
                    IsAIEnabled = request.IsEnabled
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "AI durumu güncellenirken hata oluştu",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> InitializeDefaultSettings()
        {
            try
            {
                await systemSettingsService.InitializeDefaultSettingsAsync();
                return Ok(new
                {
                    Success = true,
                    Message = "Varsayılan ayarlar başarıyla oluşturuldu"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Varsayılan ayarlar oluşturulurken hata oluştu",
                    Error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSetting([FromQuery] string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Anahtar değeri boş olamaz"
                    });
                }

                var value = await systemSettingsService.GetSettingValueAsync(key);
                return Ok(new
                {
                    Success = true,
                    Key = key,
                    Value = value,
                    Message = value != null ? "Ayar bulundu" : "Ayar bulunamadı"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Ayar alınırken hata oluştu",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetSetting([FromBody] SetSettingRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Key) || string.IsNullOrWhiteSpace(request.Value))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Anahtar ve değer alanları boş olamaz"
                    });
                }

                await systemSettingsService.SetSettingValueAsync(request.Key, request.Value, request.Description);

                return Ok(new
                {
                    Success = true,
                    Message = "Ayar başarıyla güncellendi",
                    Key = request.Key,
                    Value = request.Value
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Ayar güncellenirken hata oluştu",
                    Error = ex.Message
                });
            }
        }
    }

    public class SetAIStatusRequest
    {
        public bool IsEnabled { get; set; }
    }

    public class SetSettingRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
    }
}
