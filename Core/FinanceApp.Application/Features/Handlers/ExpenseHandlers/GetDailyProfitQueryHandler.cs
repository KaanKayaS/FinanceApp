using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.DTOs;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
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

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class GetDailyProfitQueryHandler : BaseHandler, IRequestHandler<GetDailyProfitQuery, IList<GetDailyProfitQueryResult>>
    {
        private readonly IExpenseService expenseService;
        private readonly AuthRules authRules;

        public GetDailyProfitQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger
            ,IExpenseService expenseService, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.expenseService = expenseService;
            this.authRules = authRules;
        }

        public async Task<IList<GetDailyProfitQueryResult>> Handle(GetDailyProfitQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.GetAllDailyProfitForDateTime(userId);
        }
    }
}
