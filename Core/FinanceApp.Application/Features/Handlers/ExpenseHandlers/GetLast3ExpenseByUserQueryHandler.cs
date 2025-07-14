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
    public class GetLast3ExpenseByUserQueryHandler : BaseHandler, IRequestHandler<GetLast3ExpenseByUserQuery, IList<GetLast3ExpenseByUserQueryResult>>
    {
        private readonly AuthRules authRules;

        public GetLast3ExpenseByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetLast3ExpenseByUserQueryResult>> Handle(GetLast3ExpenseByUserQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);


            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                                                                       predicate: x => x.Memberships.User.Id == userId,
                                                                       include: x => x
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x => x.User)
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(predicate: x => x.UserId == userId &&
                                                                                                              x.IsPaid == true);

            var mapInstructions = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetLast3ExpenseByUserQueryResult>>(expenses);

            var combinedList = mapPayments
                                 .Concat(mapExpenses)
                                 .Concat(mapInstructions)
                                 .OrderByDescending(x => x.PaidDate)
                                 .Take(3)
                                 .ToList();


            return combinedList;
        }
    }
}
