using FinanceApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.InvestmentPlanCommands
{
    public class CreateInvestmentPlanCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal TargetPrice { get; set; }
        public DateTime TargetDate { get; set; }
        public InvestmentCategory InvestmentCategory { get; set; }
        public InvestmentFrequency InvestmentFrequency { get; set; }
    }
}
