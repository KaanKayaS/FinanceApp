using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Features.Results.InstructionsResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class GetTodayInstructionsByUserQueryHandler : IRequestHandler<GetTodayInstructionsByUserQuery, IList<GetAllInstructionsByUserQueryResult>>
    {
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IInstructionService instructionService;

        public GetTodayInstructionsByUserQueryHandler(AuthRules authRules, IHttpContextAccessor httpContextAccessor, IInstructionService instructionService)
        {
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
            this.instructionService = instructionService;
        }

        public async Task<IList<GetAllInstructionsByUserQueryResult>> Handle(GetTodayInstructionsByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var list = await instructionService.GetTodayUnpaidInstructionsByUserAsync(userId);

            return list;
        }
    }
}
