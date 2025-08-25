using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Features.Results.InstructionsResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
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
    public class GetInstructionCountByUserQueryHandler : BaseHandler, IRequestHandler<GetInstructionCountByUserQuery, GetInstructionCountByUserQueryResult>
    {
        private readonly AuthRules authRules;
        private readonly IInstructionService instructionService;

        public GetInstructionCountByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules, IInstructionService instructionService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.instructionService = instructionService;
        }

        public async Task<GetInstructionCountByUserQueryResult> Handle(GetInstructionCountByUserQuery request, CancellationToken cancellationToken)
        {

            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await instructionService.GetInstructionSummaryByUserIdAsync(userId);
        }
    }
}
