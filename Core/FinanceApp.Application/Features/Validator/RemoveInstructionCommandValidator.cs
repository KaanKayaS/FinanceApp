using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Validator
{
    public class RemoveInstructionCommandValidator : AbstractValidator<RemoveInstructionCommand>
    {
        public RemoveInstructionCommandValidator()
        {

            RuleFor(x => x.Id)
              .GreaterThanOrEqualTo(1);
        }
    }
}
