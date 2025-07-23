using FinanceApp.Application.Features.Commands.RevokeCommands;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RevokeHandlers
{
    public class RevokeAllCommandHandler : IRequestHandler<RevokeAllCommand, Unit>
    {
        private readonly IAuthService authService;

        public RevokeAllCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<Unit> Handle(RevokeAllCommand request, CancellationToken cancellationToken)
        {
            await authService.RevokeAllRefreshTokensAsync();

            return Unit.Value;
        }
    }
}
