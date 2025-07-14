using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.RegisterCommands;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.RegisterHandlers
{
    public class RegisterCommandHandler : BaseHandler, IRequestHandler<RegisterCommand, Unit>
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly AuthRules authRules;

        public RegisterCommandHandler(UserManager<User> userManager,
            RoleManager<Role> roleManager, IMapper mapper, IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.authRules = authRules;
        }
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User? usertest = await userManager.FindByEmailAsync(request.Email);
            await authRules.UserShouldNotBeExist(usertest);

            IList<User> userList = await unitOfWork.GetReadRepository<User>().GetAllAsync();
            await authRules.UserNameMustBeUnique(request.FullName, userList);

            User user = mapper.Map<User>(request);

            user.UserName = request.Email;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) 
            {
                if (!await roleManager.RoleExistsAsync("user"))
                    await roleManager.CreateAsync(new Role
                    {
                        Name = "user",
                        NormalizedName = "USER",
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    });

                await userManager.AddToRoleAsync(user, "user");
            }

            return Unit.Value;
        }
    }
}
