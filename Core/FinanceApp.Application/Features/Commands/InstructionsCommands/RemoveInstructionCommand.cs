using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.InstructionsCommands
{
    public class RemoveInstructionCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public RemoveInstructionCommand(int id)
        {
            Id = id;
        }
    }
}
