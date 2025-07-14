using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class DigitalPlatform : EntityBase
    {
        public string Name { get; set; }
        public ICollection<SubscriptionPlan> SubscriptionPlans { get; set; }
        public ICollection<Memberships> Memberships { get; set; }
    }
}
