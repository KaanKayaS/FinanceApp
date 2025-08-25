using FinanceApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Domain.Entities
{
    public class InvestmentPlanPayment : EntityBase
    {
        public decimal Price { get; set; }
        public int InvestmentPlanId { get; set; }
        public int CreditCardId { get; set; }
        public InvestmentPlan InvestmentPlan { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
