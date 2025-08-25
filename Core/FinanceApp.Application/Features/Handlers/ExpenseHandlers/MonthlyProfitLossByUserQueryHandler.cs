using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
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

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class MonthlyProfitLossByUserQueryHandler : IRequestHandler<MonthlyProfitLossByUserQuery, MonthlyProfitLossDto>
    {
        private readonly IExpenseService expenseService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MonthlyProfitLossByUserQueryHandler(IExpenseService expenseService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.expenseService = expenseService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<MonthlyProfitLossDto> Handle(MonthlyProfitLossByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.MonthlyProfitLossByUser(userId);
        }
    }
}
