using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Commands.InstructionsCommands
{
    public class CreateInstructionCommand: IRequest<Unit>
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? Description { get; set; }
    }
}
