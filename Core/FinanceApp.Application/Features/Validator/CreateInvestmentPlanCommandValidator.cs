using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class CreateInvestmentPlanCommandValidator : AbstractValidator<CreateInvestmentPlanCommand>
    {
        public CreateInvestmentPlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Plan adı boş olamaz.")
                .MaximumLength(25).WithMessage("Plan adı en fazla 25 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(60).WithMessage("Açıklama en fazla 60 karakter olabilir.");

            RuleFor(x => x.TargetPrice)
                .GreaterThan(2000).WithMessage("Hedef tutar 2000'den büyük olmalıdır.")
                .LessThanOrEqualTo(100000000).WithMessage("Hedef tutar 100.000.000'dan büyük olmamalıdır.");

            RuleFor(x => x.TargetDate)
                .Must(BeAtLeastOneWeekFromNow).WithMessage("Hedef tarih bugünden en az 7 gün sonrası olmalıdır.");

            RuleFor(x => x.InvestmentCategory)
                .IsInEnum().WithMessage("Geçersiz yatırım kategorisi.");

            RuleFor(x => x.InvestmentFrequency)
                .IsInEnum().WithMessage("Geçersiz yatırım sıklığı.");
        }

        private bool BeAtLeastOneWeekFromNow(DateTime targetDate)
        {
            return targetDate.Date >= DateTime.UtcNow.Date.AddDays(7);
        }
    }
}
