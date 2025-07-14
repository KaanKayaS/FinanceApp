using FinanceApp.Application.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Exceptions
{
    public class SubscriptionPlanNotFoundException : BaseException
    {
        public SubscriptionPlanNotFoundException() : base("Böyle bir plan bulunamadı!"){ }
    }
}
