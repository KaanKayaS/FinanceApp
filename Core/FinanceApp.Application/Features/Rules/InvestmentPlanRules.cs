using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class InvestmentPlanRules : BaseRules
    {
        public virtual Task PlanNotFound(InvestmentPlan plan)
        {
            if (plan == null)
                throw new PlanNotFoundException();

            return Task.CompletedTask;
        }

        public virtual Task DoesThisPlanBelongToYou(InvestmentPlan plan, int userId)
        {
            if (plan.UserId != userId) throw new DoesThisPlanBelongToYouException();
            return Task.CompletedTask;
        }
    }
}
