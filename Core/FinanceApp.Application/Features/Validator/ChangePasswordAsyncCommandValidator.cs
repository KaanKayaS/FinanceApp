using FinanceApp.Application.Features.Commands.ChangePasswordCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class ChangePasswordAsyncCommandValidator : AbstractValidator<ChangePasswordAsyncCommand>
    {
        public ChangePasswordAsyncCommandValidator()
        {

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .WithName("Parola");


            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Equal(x => x.NewPassword)
                .WithMessage("'Parola Tekrarı',Parola değerine eşit olmalı.")
                .WithName("Parola Tekrarı");
        }
    }
}
