using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class InvestmentPlan : EntityBase
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public decimal TargetPrice { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public User User { get; set; }
        public ICollection<InvestmentPlanPayment> InvestmentPlanPayments { get; set; }
        public InvestmentCategory InvestmentCategory { get; set; }
        public InvestmentFrequency InvestmentFrequency { get; set; }
    }
}
