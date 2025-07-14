using FinanceApp.Application.Features.Commands.RevokeCommands;
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
        private readonly UserManager<User> userManager;

        public RevokeAllCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Unit> Handle(RevokeAllCommand request, CancellationToken cancellationToken)
        {
            var users = await userManager.Users.ToListAsync(cancellationToken);

            foreach (var user in users) 
            {
                user.RefreshToken = null;
                await userManager.UpdateAsync(user);
            }

            return Unit.Value;
        }
    }
}
