using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class InstructionRules : BaseRules
    {
        public virtual Task InstructionNameNotMustBeSame(IList<Instructions> instructions, string title, DateTime scheduledDate)
        {
            foreach (var instruction in instructions)
            {
                var instructionDate = instruction.CreatedDate.Date;
                var today = DateTime.UtcNow.Date;

                if (instruction.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && instruction.ScheduledDate.Date == scheduledDate.Date)
                    throw new InstructionNameNotMustBeSameException();

            }
            return Task.CompletedTask;
        }

        public virtual Task InstructionsNotFound(Instructions instructions)
        {
        
            if (instructions == null)
                throw new InstructionsNotFoundException();
          
            return Task.CompletedTask;
        }

        public virtual Task IsThisYourInstruction(Instructions instructions, int userId)
        {

            if (instructions.UserId != userId)
                throw new IsThisYourInstructionException();

            return Task.CompletedTask;
        }
    }
}
