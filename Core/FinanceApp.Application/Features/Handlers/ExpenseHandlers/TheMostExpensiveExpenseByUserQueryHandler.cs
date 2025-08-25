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
    public class TheMostExpensiveExpenseByUserQueryHandler : IRequestHandler<TheMostExpensiveExpenseByUserQuery, TheMostExpensiveExpenseDto>
    {
        private readonly IExpenseService expenseService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TheMostExpensiveExpenseByUserQueryHandler(IExpenseService expenseService, AuthRules authRules, IHttpContextAccessor httpContextAccessor)
        {
            this.expenseService = expenseService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<TheMostExpensiveExpenseDto> Handle(TheMostExpensiveExpenseByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.TheMostExpensiveExpenseByUser(userId);
        }
    }
}
