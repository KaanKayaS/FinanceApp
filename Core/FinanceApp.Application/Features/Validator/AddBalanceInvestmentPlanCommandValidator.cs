using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class AddBalanceInvestmentPlanCommandValidator : AbstractValidator<AddBalanceInvestmentPlanCommand>
    {
        public AddBalanceInvestmentPlanCommandValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Girilen miktar 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100000000).WithMessage("Girilen miktar 0'dan büyük olmalıdır.");

            RuleFor(x => x.CardId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kredi kartı seçilmelidir.");

            RuleFor(x => x.InvestmentPlanId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir yatırım planı seçilmelidir.");
        }
    }
}
