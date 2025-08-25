using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Application.Features.Results.UserResults;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<int> GetUserCountAsync(int userId)
        {
            var user = await userManager.Users.ToListAsync();

            return user.Count;
        }

        public async Task<GetUserInfoByIdQueryResult> GetUserInfoById(int userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

           return new GetUserInfoByIdQueryResult
           {
                Email = user.Email,
                FullName = user.FullName
           };

        }
    }
}
