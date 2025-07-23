using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IMembershipService
    {
        Task CreateMembershipAsync(int userId, int creditCardId, int digitalPlatformId, SubscriptionType subscriptionType);
        Task<IList<GetAllMembershipsByUserQueryResult>> GetAllByUserAsync(int userId);
        Task<int> GetMembershipCountByUserAsync(int userId);
        Task RemoveMembershipAsync(int userId, int digitalPlatformId);
    }
}
