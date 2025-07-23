using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class GetLastMonthExpenseTotalAmountQueryHandler : BaseHandler, IRequestHandler<GetLastMonthExpenseTotalAmountQuery, decimal>
    {
        private readonly AuthRules authRules;
        private readonly IExpenseService expenseService;

        public GetLastMonthExpenseTotalAmountQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules, IExpenseService expenseService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.expenseService = expenseService;
        }

        public async Task<decimal> Handle(GetLastMonthExpenseTotalAmountQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.GetLastMonthExpenseTotalAmountAsync(userId);
        }
    }
}
