using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Application.Features.Results.UserResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<GetUserInfoByIdQueryResult> GetUserInfoById(int userId);
        Task<int> GetUserCountAsync(int userId);
    }
}
