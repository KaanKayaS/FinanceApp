using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Lütfen isim alanını doldurunuz")
                .MinimumLength(3)
                .MaximumLength(25);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Lütfen fiyat alanını doldurunuz")
                .GreaterThan(0)
                .LessThanOrEqualTo(1000000).WithMessage("Fiyat değeri 1.000.000' dan küçük olmalıdır.");
        }
    }
}
