using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class RefreshTokenRules :BaseRules
    {
        public Task RefreshTokenShouldNotBeExpired(DateTime? expiryDate)
        {
            if (expiryDate <= DateTime.UtcNow)
            {
                throw new RefreshTokenShouldNotBeExpiredException();
            }
            return Task.CompletedTask;
        }
    }
}
