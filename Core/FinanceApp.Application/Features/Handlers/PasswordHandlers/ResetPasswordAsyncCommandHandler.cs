using FinanceApp.Application.Features.Commands.PasswordCommands;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.PasswordHandlers
{
    public class ResetPasswordAsyncCommandHandler : IRequestHandler<ResetPasswordAsyncCommand, Unit>
    {
        private readonly IAuthService authService;

        public ResetPasswordAsyncCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<Unit> Handle(ResetPasswordAsyncCommand request, CancellationToken cancellationToken)
        {
            await authService.ResetPasswordAsync(request);
            return Unit.Value;
        }
    }
}
