using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.InstructionsCommands
{
    public class SetPaidTrueInstructionCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public SetPaidTrueInstructionCommand(int id)
        {
            Id = id;
        }
    }
}
