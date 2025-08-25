using FinanceApp.Application.Features.Commands.ChangePasswordCommands;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.ChangePasswordHandlers
{
    public class ChangePasswordAsyncCommandHandler : IRequestHandler<ChangePasswordAsyncCommand, Unit>
    {
        private readonly IAuthService authService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ChangePasswordAsyncCommandHandler(IAuthService authService,AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.authService = authService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }
        async Task<Unit> IRequestHandler<ChangePasswordAsyncCommand, Unit>.Handle(ChangePasswordAsyncCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            string stringUserId = userId.ToString();
            await authService.ChangePasswordAsync(stringUserId, request);

            return Unit.Value;
        }
    }
}
