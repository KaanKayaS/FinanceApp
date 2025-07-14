using FinanceApp.Application.Features.Commands.MembershipsCommands;
using FinanceApp.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class RemoveMembershipCommandValidator : AbstractValidator<RemoveMembershipCommand>
    {
        public RemoveMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1);
        }
    }
}
