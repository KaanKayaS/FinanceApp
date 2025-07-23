using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Results.InstructionsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Interfaces.Services
{
    public interface IInstructionService
    {
        Task<GetInstructionCountByUserQueryResult> GetInstructionSummaryByUserIdAsync(int userId);
        Task CreateInstructionAsync(CreateInstructionCommand request, int userId);
        Task<IList<GetAllInstructionsByUserQueryResult>> GetAllUnpaidInstructionsByUserAsync(int userId);
        Task RemoveInstructionAsync(int instructionId, int userId);
        Task<bool> SetInstructionPaidTrueAsync(int instructionId, int userId);
        Task UpdateInstructionAsync(UpdateInstructionCommand request, int userId);
    }
}
