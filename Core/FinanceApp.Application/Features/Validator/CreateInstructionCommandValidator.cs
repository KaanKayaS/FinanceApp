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
                .MinimumLength(4).WithMessage("Başlık en az 4 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Başlık en fazla 20 karakter olmalıdır.");


            RuleFor(x => x.ScheduledDate)
                .Must(BeInTheFuture).WithMessage("Planlanan tarih bugünden sonra olmalıdır.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(1000000).WithMessage("Fiyat değeri 1.000.000' dan küçük olmalıdır.");

            RuleFor(x => x.Description)
                .MinimumLength(4).WithMessage("Açıklama en az 4 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Başlık en fazla 20 karakter olmalıdır.");


        }

        private bool BeInTheFuture(DateTime date)
        {
            return date.Date > DateTime.Today;
        }

        
    }
}
