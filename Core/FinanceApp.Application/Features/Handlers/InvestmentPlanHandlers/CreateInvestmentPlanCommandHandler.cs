using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
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

namespace FinanceApp.Application.Features.Handlers.InvestmentPlanHandlers
{
    public class CreateInvestmentPlanCommandHandler : IRequestHandler<CreateInvestmentPlanCommand, Unit>
    {
        private readonly IInvestmentPlanService planService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CreateInvestmentPlanCommandHandler(IInvestmentPlanService planService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.planService = planService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(CreateInvestmentPlanCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await planService.CreateAsync(request, userId);

            return Unit.Value;
        }
    }
}
