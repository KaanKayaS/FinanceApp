using FinanceApp.Application.Features.Results.SubscriptionPlansResults;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public SubscriptionPlanService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<GetAllSubscriptionPlansQueryResult> GetPlanPriceAsync(int digitalPlatformId, SubscriptionType planType)
        {
            var subPlan = await unitOfWork.GetReadRepository<SubscriptionPlan>()
                       .GetAsync(x => x.DigitalPlatformId == digitalPlatformId && x.PlanType == planType);

            if (subPlan == null)
                throw new Exception("Ödeme planı bulunamadı");

            return new GetAllSubscriptionPlansQueryResult
            {
                Price = subPlan.Price
            };
        }
    }
}
