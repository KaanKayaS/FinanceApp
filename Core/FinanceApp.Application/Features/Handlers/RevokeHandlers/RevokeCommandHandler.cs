using FinanceApp.Application.Features.Commands.RevokeCommands;
using FinanceApp.Application.Features.Rules;
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
        private readonly UserManager<User> userManager;
        private readonly AuthRules authRules;

        public RevokeCommandHandler(UserManager<User> userManager, AuthRules authRules)
        {
            this.userManager = userManager;
            this.authRules = authRules;
        }
        public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            User? user = await userManager.FindByEmailAsync(request.Email);
            await authRules.EmailAddressShouldBeValid(user);

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
