using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class AddBalanceCreditCardCommandValidator : AbstractValidator<AddBalanceCreditCardCommand>
    {
        public AddBalanceCreditCardCommandValidator()
        {
            RuleFor(x => x.Id)
                 .GreaterThan(0);

            RuleFor(x => x.Balance)
                    .GreaterThan(0).WithMessage("Yüklenecek bakiye 0'dan büyük olmalıdır.")
                    .LessThanOrEqualTo(1000000).WithMessage("Yüklenecek bakiye en fazla 1.000.000 TL olabilir.");

            RuleFor(x => x.Name)
                    .MinimumLength(3)
                    .MaximumLength(30);
        }
    }
}
