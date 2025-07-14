using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class SubscriptionPlan : EntityBase
    {
        public int DigitalPlatformId { get; set; }
        public SubscriptionType PlanType { get; set; } // Aylık, Yıllık vb.
        public decimal Price { get; set; }
        public DigitalPlatform DigitalPlatform { get; set; }
        public ICollection<Memberships> Memberships { get; set; }
    }
}
