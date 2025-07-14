using FinanceApp.Application.Features.Commands.RegisterCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(2)
               .WithName("İsim soyisim");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(60)
                .EmailAddress()
                .MinimumLength(8)
                .WithName("E-posta adresi");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithName("Parola");


            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Equal(x => x.Password)
                .WithMessage("'Parola Tekrarı',Parola değerine eşit olmalı.")
                .WithName("Parola Tekrarı");
        }
    }
}
