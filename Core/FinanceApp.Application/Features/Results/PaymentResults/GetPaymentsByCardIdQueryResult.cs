using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Results.PaymentResults
{
    public class GetPaymentsByCardIdQueryResult
    {
        public string DigitalPlatformName { get; set; }
        public string? SubscriptionPlanName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public AddBalanceCategory? AddBalanceCategory { get; set; }
    }
}
