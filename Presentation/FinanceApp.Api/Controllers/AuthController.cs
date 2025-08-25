using FinanceApp.Application.Features.Commands.ChangePasswordCommands;
using FinanceApp.Application.Features.Commands.LoginCommands;
using FinanceApp.Application.Features.Commands.PasswordCommand;
using FinanceApp.Application.Features.Commands.PasswordCommands;
using FinanceApp.Application.Features.Commands.RefreshTokenCommands;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Commands.RevokeCommands;
using FinanceApp.Application.Hubs;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.AI;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly UserManager<User> userManager;

        public AuthController(IMediator mediator, UserManager<User> userManager)
        {
            this.mediator = mediator;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Hesabınız başarıyla oluşturuldu. Lütfen giriş yapmadan mailinize gelen link ile mailinizi doğrulayınız.");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var response = await mediator.Send(command);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Revoke(RevokeCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RevokeAll()
        {
            await mediator.Send(new RevokeAllCommand());
            return Ok();
        }


        [HttpPost]
        [EnableRateLimiting("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordAsyncCommand command)
        {
            var response = await mediator.Send(command);
            return Ok("Mailinize gelen link ile şifrenizi değiştirebilirsiniz.");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordAsyncCommand command)
        {
            var response = await mediator.Send(command);
            return Ok("Şifreniz başarıyla değiştirildi");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordAsyncCommand command)
        {
            var response = await mediator.Send(command);
            return Ok("Şifreniz başarıyla değiştirildi");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Kullanıcı bulunamadı");

            var decodedToken = WebUtility.UrlDecode(token).Trim();

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return Ok("Email başarıyla onaylandı.");
            else
                return BadRequest("Email onaylama başarısız.");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                // Kullanıcı ID'sini al
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    // AIHub'dan kullanıcı verilerini temizle
                    AIHub.CleanupUserData(userId);
                    
                    // Chat history'yi temizle
                    HistoryService.RemoveChatHistory(userId);
                    
                    Console.WriteLine($"User logged out successfully: {userId}");
                }
                
                return Ok(new { message = "Başarıyla çıkış yapıldı" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                return StatusCode(500, new { message = "Çıkış yapılırken hata oluştu" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSystemStatus()
        {
            try
            {
                var activeConnections = AIHub.GetActiveConnectionCount();
                var activeUserIds = AIHub.GetActiveUserIds().ToList();
                var activeHistoryCount = HistoryService.GetActiveHistoryCount();
                var activeHistoryUserIds = HistoryService.GetActiveUserIds().ToList();

                return Ok(new
                {
                    activeConnections,
                    activeUsers = activeUserIds,
                    activeHistoryCount,
                    activeHistoryUsers = activeHistoryUserIds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetSystemStatus error: {ex.Message}");
                return StatusCode(500, new { message = "Sistem durumu alınırken hata oluştu" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ClearAllData()
        {
            try
            {
                // Tüm chat history'leri temizle
                HistoryService.ClearAllHistories();
                
                // Tüm bağlantıları temizle (AIHub'da zaten var ama manuel olarak da yapabiliriz)
                var activeUserIds = AIHub.GetActiveUserIds().ToList();
                foreach (var userId in activeUserIds)
                {
                    AIHub.CleanupUserData(userId);
                }
                
                Console.WriteLine("All system data cleared successfully");
                
                return Ok(new { message = "Tüm sistem verileri temizlendi" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClearAllData error: {ex.Message}");
                return StatusCode(500, new { message = "Veriler temizlenirken hata oluştu" });
            }
        }
    }
}
