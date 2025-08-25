using AutoMapper;
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
    public class GetDailyExpenseListByUserQueryHandler : IRequestHandler<GetDailyExpenseListByUserQuery, IList<DailyIncomeExpenseDto>>
    {
        private readonly IExpenseService expenseService;
        private readonly AuthRules authRules;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public GetDailyExpenseListByUserQueryHandler(IExpenseService expenseService, AuthRules authRules, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.expenseService = expenseService;
            this.authRules = authRules;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;

        }

        public async Task<IList<DailyIncomeExpenseDto>> Handle(GetDailyExpenseListByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.GetAllExpenseForDateTime(userId);    
        }
    }
}
