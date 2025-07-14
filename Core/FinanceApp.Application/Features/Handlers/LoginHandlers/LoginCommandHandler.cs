using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.LoginCommands;
using FinanceApp.Application.Features.Results.LoginResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Tokens;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.LoginHandlers
{
    public class LoginCommandHandler : BaseHandler, IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly AuthRules authRules;
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public LoginCommandHandler(AuthRules authRules, UserManager<User> userManager,
            IMapper mapper, IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IConfiguration configuration) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }
        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await userManager.FindByEmailAsync(request.Email);
            bool checkPassword = await userManager.CheckPasswordAsync(user, request.Password);

           
            await authRules.EmailOrPasswordShouldNotBeInvalid(user, checkPassword);

            var roles = await userManager.GetRolesAsync(user);

            JwtSecurityToken token = await tokenService.CreateToken(user, roles);
            string refreshToken = tokenService.GenerateRefreshToken();


            int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int RefreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(RefreshTokenValidityInDays);

            await userManager.UpdateAsync(user);
            await userManager.UpdateSecurityStampAsync(user);

            string _token = new JwtSecurityTokenHandler().WriteToken(token);

            await userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", _token);

            return new()
            {
                Token = _token,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }
    }
}
