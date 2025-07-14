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
    public class GetLastMonthExpenseTotalAmountQueryHandler : BaseHandler, IRequestHandler<GetLastMonthExpenseTotalAmountQuery, decimal>
    {
        private readonly AuthRules authRules;

        public GetLastMonthExpenseTotalAmountQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<decimal> Handle(GetLastMonthExpenseTotalAmountQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);


            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId && x.CreatedDate >= oneMonthAgo);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                                                                       predicate: x => x.Memberships.User.Id == userId && x.CreatedDate >= oneMonthAgo,
                                                                       include: x => x
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x => x.User)
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(predicate: x => x.UserId == userId &&
                                                                                                              x.IsPaid == true 
                                                                                                              && x.CreatedDate >= oneMonthAgo);

            var mapInstructions = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(expenses);

            var combinedList = mapPayments
                                 .Concat(mapExpenses)
                                 .Concat(mapInstructions)
                                 .ToList();

            var totalAmount = combinedList.Sum(x => x.Amount);

            return totalAmount;
        }
    }
}
