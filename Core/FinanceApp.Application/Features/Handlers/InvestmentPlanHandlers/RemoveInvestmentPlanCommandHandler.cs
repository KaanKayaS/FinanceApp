using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
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

namespace FinanceApp.Application.Features.Handlers.InvestmentPlanHandlers
{
    public class RemoveInvestmentPlanCommandHandler : IRequestHandler<RemoveInvestmentPlanCommand, Unit>
    {
        private readonly IInvestmentPlanService planService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AuthRules authRules;

        public RemoveInvestmentPlanCommandHandler(IInvestmentPlanService planService, IHttpContextAccessor httpContextAccessor, AuthRules authRules)
        {
            this.planService = planService;
            this.httpContextAccessor = httpContextAccessor;
            this.authRules = authRules;
        }
        public async Task<Unit> Handle(RemoveInvestmentPlanCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await planService.RemoveInvestmentAsync(request.Id, userId);

            return Unit.Value;
        }
    }
}
