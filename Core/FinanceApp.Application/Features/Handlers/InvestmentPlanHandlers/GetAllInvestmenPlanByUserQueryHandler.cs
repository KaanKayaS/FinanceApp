using FinanceApp.Application.Features.Queries.InvestmentPlanQueries;
using FinanceApp.Application.Features.Results.InvestmentPlanResults;
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
    public class GetAllInvestmenPlanByUserQueryHandler : IRequestHandler<GetAllInvestmenPlanByUserQuery, IList<GetAllInvestmenPlanByUserQueryResult>>
    {
        private readonly IInvestmentPlanService planService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllInvestmenPlanByUserQueryHandler(IInvestmentPlanService planService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.planService = planService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IList<GetAllInvestmenPlanByUserQueryResult>> Handle(GetAllInvestmenPlanByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var list = await planService.GetAllPlanByUserAsync(userId);

            return list;
        }
    }
}
