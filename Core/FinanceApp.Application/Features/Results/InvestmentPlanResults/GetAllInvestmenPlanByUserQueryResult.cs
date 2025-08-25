using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.InvestmentPlanResults
{
    public class GetAllInvestmenPlanByUserQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal TargetPrice { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public InvestmentCategory InvestmentCategory { get; set; }
        public InvestmentFrequency InvestmentFrequency { get; set; }
        public decimal PerPaymentAmount { get; set; }
        public int HowManyDaysLeft { get; set; }
    }
}
