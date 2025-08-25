using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class CreateCreditCardCommandValidator : AbstractValidator<CreateCreditCardCommand>
    {   
            public CreateCreditCardCommandValidator()
            {
                RuleFor(x => x.CardNo)
                    .NotEmpty().WithMessage("Kart numarası boş olamaz.")
                    .Length(16).WithMessage("Kart numarası 16 haneli olmalıdır.")
                    .Matches(@"^\d{16}$").WithMessage("Kart numarası yalnızca rakamlardan oluşmalıdır.");

                RuleFor(x => x.ValidDate)
                    .NotEmpty().WithMessage("Son kullanma tarihi boş olamaz.")
                    .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$").WithMessage("Son kullanma tarihi MM/YY formatında olmalıdır.")
                    .Must(BeAValidExpiryDate).WithMessage("Son kullanma tarihi geçmiş olamaz.");

                RuleFor(x => x.NameOnCard)
                    .NotEmpty().WithMessage("Kart üzerindeki isim boş olamaz.")
                    .MaximumLength(25).WithMessage("Kart üzerindeki isim en fazla 25 karakter olabilir.");

                RuleFor(x => x.Cvv)
                    .NotEmpty().WithMessage("CVV boş olamaz.")
                    .Matches(@"^\d{3,4}$").WithMessage("CVV 3 veya 4 haneli olmalıdır.");
            }

            private bool BeAValidExpiryDate(string validDate)
            {
                if (string.IsNullOrWhiteSpace(validDate)) return false;

                var parts = validDate.Split('/');
                if (parts.Length != 2) return false;

                if (!int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
                    return false;

                var fullYear = 2000 + year;
                if (month < 1 || month > 12) return false;

                var lastDayOfMonth = new DateTime(fullYear, month, DateTime.DaysInMonth(fullYear, month));

                return lastDayOfMonth >= DateTime.Now.Date;
            }
        }

}


