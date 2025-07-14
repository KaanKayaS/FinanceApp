using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.ExpenseHandlers
{
    public class GetAllExpenseByUserQueryHandler : BaseHandler, IRequestHandler<GetAllExpenseByUserQuery, IList<GetAllExpenseByUserQueryResult>>
    {
        private readonly AuthRules authRules;

        public GetAllExpenseByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor
            ,AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetAllExpenseByUserQueryResult>> Handle(GetAllExpenseByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);

            return mapper.Map<IList<GetAllExpenseByUserQueryResult>>(expenses);
        }
    }
}
