using FinanceApp.Application.Features.Results.SubscriptionPlansResults;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface ISubscriptionPlanService
    {
        Task<GetAllSubscriptionPlansQueryResult> GetPlanPriceAsync(int digitalPlatformId, SubscriptionType planType);
    }
}
