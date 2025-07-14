using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class RemoveCreditCardCommandValidator : AbstractValidator<RemoveCreditCardCommand>
    {
        public RemoveCreditCardCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1);
        }
    }
}
