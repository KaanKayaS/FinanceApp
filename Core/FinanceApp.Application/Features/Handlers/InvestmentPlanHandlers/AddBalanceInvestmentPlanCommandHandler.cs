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
    public class AddBalanceInvestmentPlanCommandHandler : IRequestHandler<AddBalanceInvestmentPlanCommand, Unit>
    {
        private readonly IInvestmentPlanService investmentPlanService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AddBalanceInvestmentPlanCommandHandler(IInvestmentPlanService investmentPlanService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.investmentPlanService = investmentPlanService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AddBalanceInvestmentPlanCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await investmentPlanService.AddBalancePlan(request, userId);

            return Unit.Value;
        }
    }
}
