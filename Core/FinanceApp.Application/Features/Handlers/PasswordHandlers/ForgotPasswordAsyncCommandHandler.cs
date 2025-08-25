using FinanceApp.Application.Features.Commands.PasswordCommand;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.PasswordHandlers
{
    public class ForgotPasswordAsyncCommandHandler : IRequestHandler<ForgotPasswordAsyncCommand, Unit>
    {
        private readonly IAuthService authService;

        public ForgotPasswordAsyncCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<Unit> Handle(ForgotPasswordAsyncCommand request, CancellationToken cancellationToken)
        {
            await authService.ForgotPasswordAsync(request.Email);
            return Unit.Value;
        }
    }
}
