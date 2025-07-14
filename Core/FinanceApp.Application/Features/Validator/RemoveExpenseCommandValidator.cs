using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class RemoveExpenseCommandValidator : AbstractValidator<RemoveExpenseCommand>
    {
        public RemoveExpenseCommandValidator()
        {
            RuleFor(x => x.Id)
              .GreaterThanOrEqualTo(1);
        }
    }
}
