using FinanceApp.Application.Features.Commands.LoginCommands;
using FinanceApp.Application.Features.Commands.RefreshTokenCommands;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Commands.RevokeCommands;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, "Hesabınız başarıyla oluşturuldu");
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
        public async Task<IActionResult> Revoke(RevokeCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RevokeAll()
        {
            await mediator.Send(new RevokeAllCommand());
            return Ok();
        }
    }
}
