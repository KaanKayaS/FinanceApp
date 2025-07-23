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
    public class GetLast3ExpenseByUserQueryHandler : BaseHandler, IRequestHandler<GetLast3ExpenseByUserQuery, IList<GetLast3ExpenseByUserQueryResult>>
    {
        private readonly AuthRules authRules;
        private readonly IExpenseService expenseService;

        public GetLast3ExpenseByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules, IExpenseService expenseService) : base(mapper, unitOfWork, httpContextAccessor , logger)
        {
            this.authRules = authRules;
            this.expenseService = expenseService;
        }

        public async Task<IList<GetLast3ExpenseByUserQueryResult>> Handle(GetLast3ExpenseByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await expenseService.GetLast3ExpenseByUserAsync(userId);
        }
    }
}
