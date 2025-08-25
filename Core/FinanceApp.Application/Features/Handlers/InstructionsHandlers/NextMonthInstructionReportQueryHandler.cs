using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class NextMonthInstructionReportQueryHandler : IRequestHandler<NextMonthInstructionReportQuery, Unit>
    {
        private readonly IInstructionService instructionService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NextMonthInstructionReportQueryHandler(IInstructionService instructionService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.instructionService = instructionService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(NextMonthInstructionReportQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await instructionService.GeneratePdfGetUnpaidInstructiionsNextMonthReport(userId);

            return Unit.Value;
        }
    }
}
