using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class CreateInstructionCommandValidator : AbstractValidator<CreateInstructionCommand>
    {
        
        public CreateInstructionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MinimumLength(4).WithMessage("Başlık en az 4 karakter olmalıdır.");

            RuleFor(x => x.ScheduledDate)
                .Must(BeInTheFuture).WithMessage("Planlanan tarih bugünden sonra olmalıdır.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır.");
        }

        private bool BeInTheFuture(DateTime date)
        {
            return date.Date > DateTime.Today;
        }

        
    }
}
