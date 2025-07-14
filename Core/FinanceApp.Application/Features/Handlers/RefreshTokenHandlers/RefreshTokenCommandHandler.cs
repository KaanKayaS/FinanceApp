using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.RefreshTokenCommands;
using FinanceApp.Application.Features.Results.RefreshTokenResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Tokens;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RefreshTokenHandlers
{
    public class RefreshTokenCommandHandler : BaseHandler, IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly RefreshTokenRules refreshTokenRules;

        public RefreshTokenCommandHandler(UserManager<User> userManager,
            IMapper mapper, IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor, ITokenService tokenService, RefreshTokenRules refreshTokenRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.refreshTokenRules = refreshTokenRules;
        }
        public async Task<RefreshTokenCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            string email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByNameAsync(email);
            var roles = await userManager.GetRolesAsync(user);

            await refreshTokenRules.RefreshTokenShouldNotBeExpired(user.RefreshTokenExpiryTime);

            JwtSecurityToken newAccessToken = await tokenService.CreateToken(user, roles);
            string NewRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = NewRefreshToken;
            await userManager.UpdateAsync(user);

            return new RefreshTokenCommandResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = NewRefreshToken
            };
        }
    }
}
