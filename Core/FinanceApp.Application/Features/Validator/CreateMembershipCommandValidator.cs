using FinanceApp.Application.Features.Commands.MembershipsCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class CreateMembershipCommandValidator : AbstractValidator<CreateMembershipCommand>
    {
        public CreateMembershipCommandValidator()
        {
            RuleFor(x => x.DigitalPlatformId).GreaterThan(0).WithMessage("Platform seçilmeli.");
            RuleFor(x => x.CreditCardId).GreaterThan(0).WithMessage("Kart seçilmeli.");
        }
    }
}
