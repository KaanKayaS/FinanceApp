using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.InvestmentPlanCommands
{
    public class AddBalanceInvestmentPlanCommand : IRequest<Unit>
    {
        public decimal Price { get; set; }
        public int CardId { get; set; }
        public int InvestmentPlanId { get; set; }
    }
}
