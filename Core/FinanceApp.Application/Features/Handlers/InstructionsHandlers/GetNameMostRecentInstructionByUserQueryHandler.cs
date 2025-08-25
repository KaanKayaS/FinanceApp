using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class GetNameMostRecentInstructionByUserQueryHandler : IRequestHandler<GetNameMostRecentInstructionByUserQuery, InstructionDto>
    {
        private readonly IInstructionService instructionService;

        public GetNameMostRecentInstructionByUserQueryHandler(IInstructionService instructionService)
        {
            this.instructionService = instructionService;
        }

        public async Task<InstructionDto> Handle(GetNameMostRecentInstructionByUserQuery request, CancellationToken cancellationToken)
        {
            return await instructionService.GetNameMostRecentInstructionByUserAsync();
        }
    }
}
