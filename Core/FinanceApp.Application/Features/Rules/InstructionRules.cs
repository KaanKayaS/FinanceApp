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
        public Task InstructionNameNotMustBeSame(IList<Instructions> instructions, string title)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                    throw new InstructionNameNotMustBeSameException();

            }
            return Task.CompletedTask;
        }

        public Task InstructionsNotFound(Instructions instructions)
        {
        
            if (instructions == null)
                throw new InstructionsNotFoundException();
          
            return Task.CompletedTask;
        }

        public Task IsThisYourInstruction(Instructions instructions, int userId)
        {

            if (instructions.UserId != userId)
                throw new IsThisYourInstructionException();

            return Task.CompletedTask;
        }
    }
}
