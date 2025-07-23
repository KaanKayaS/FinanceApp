using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Results.LoginResults;
using FinanceApp.Application.Features.Results.RefreshTokenResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginCommandResult> LoginAsync(string email, string password);
        Task<RefreshTokenCommandResult> RefreshTokenAsync(string accessToken, string refreshToken);
        Task RegisterAsync(RegisterCommand request);
        Task RevokeAllRefreshTokensAsync();
        Task RevokeRefreshTokenAsync(string email);
    }
}
