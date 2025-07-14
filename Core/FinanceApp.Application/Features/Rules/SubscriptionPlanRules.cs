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
    public class SubscriptionPlanRules : BaseRules
    {
        public Task SubscriptionPlanNotFound(SubscriptionPlan plan)
        {
            if (plan == null) throw new SubscriptionPlanNotFoundException();
            return Task.CompletedTask;
        }
    }
}
