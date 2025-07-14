using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class Memberships : EntityBase
    {
        public int DigitalPlatformId { get; set; }
        public int UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public User User { get; set; }
        public DigitalPlatform DigitalPlatform { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
