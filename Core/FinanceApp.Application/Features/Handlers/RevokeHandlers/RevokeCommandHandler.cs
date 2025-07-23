using FinanceApp.Application.Features.Commands.RevokeCommands;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RevokeHandlers
{
    public class RevokeCommandHandler : IRequestHandler<RevokeCommand, Unit>
    {
        private readonly IAuthService authService;

        public RevokeCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            await authService.RevokeRefreshTokenAsync(request.Email);
            return Unit.Value;
        }
    }
}
