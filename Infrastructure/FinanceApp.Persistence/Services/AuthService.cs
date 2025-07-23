using AutoMapper;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Results.LoginResults;
using FinanceApp.Application.Features.Results.RefreshTokenResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.Tokens;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        private readonly AuthRules authRules;
        private readonly RefreshTokenRules refreshTokenRules;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthService(UserManager<User> userManager, RoleManager<Role> roleManager, ITokenService tokenService,
            IConfiguration configuration, AuthRules authRules, RefreshTokenRules refreshTokenRules, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
            this.configuration = configuration;
            this.authRules = authRules;
            this.refreshTokenRules = refreshTokenRules;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<LoginCommandResult> LoginAsync(string email, string password)
        {
            User? user = await userManager.FindByEmailAsync(email);
            bool validPassword = await userManager.CheckPasswordAsync(user, password);

            await authRules.EmailOrPasswordShouldNotBeInvalid(user, validPassword);

            var roles = await userManager.GetRolesAsync(user);

            JwtSecurityToken token = await tokenService.CreateToken(user, roles);
            string refreshToken = tokenService.GenerateRefreshToken();

            int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenDays);

            await userManager.UpdateAsync(user);
            await userManager.UpdateSecurityStampAsync(user);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            await userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", accessToken);

            return new LoginCommandResult
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }

        public async Task<RefreshTokenCommandResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            string email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByNameAsync(email);
            var roles = await userManager.GetRolesAsync(user);

            await refreshTokenRules.RefreshTokenShouldNotBeExpired(user.RefreshTokenExpiryTime);

            JwtSecurityToken newAccessToken = await tokenService.CreateToken(user, roles);
            string newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return new RefreshTokenCommandResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task RegisterAsync(RegisterCommand request)
        {
            User? existingUser = await userManager.FindByEmailAsync(request.Email);
            await authRules.UserShouldNotBeExist(existingUser);

            IList<User> userList = await unitOfWork.GetReadRepository<User>().GetAllAsync();
            await authRules.UserNameMustBeUnique(request.FullName, userList);

            User user = mapper.Map<User>(request);

            user.UserName = request.Email;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Kullanıcı oluşturulamadı: {errors}");
            }

            if (!await roleManager.RoleExistsAsync("user"))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "user",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                });
            }

            await userManager.AddToRoleAsync(user, "user");
        }

        public async Task RevokeAllRefreshTokensAsync()
        {
            var users = await userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                user.RefreshToken = null;
                await userManager.UpdateAsync(user);
            }
        }

        public async Task RevokeRefreshTokenAsync(string email)
        {
            User? user = await userManager.FindByEmailAsync(email);
            await authRules.EmailAddressShouldBeValid(user);

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);
        }
    }
}
