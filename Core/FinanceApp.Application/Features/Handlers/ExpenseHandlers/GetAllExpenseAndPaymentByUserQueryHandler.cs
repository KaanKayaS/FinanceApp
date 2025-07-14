using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.ExpenseQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.MembershipResult;
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
    public class GetAllExpenseAndPaymentByUserQueryHandler : BaseHandler, IRequestHandler<GetAllExpenseAndPaymentByUserQuery,
        IList<GetAllExpenseAndPaymentByUserQueryResult>>
    {
        private readonly AuthRules authRules;

        public GetAllExpenseAndPaymentByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetAllExpenseAndPaymentByUserQueryResult>> Handle(GetAllExpenseAndPaymentByUserQuery request, CancellationToken cancellationToken)
         {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var expenses = await unitOfWork.GetReadRepository<Expens>().GetAllAsync(x => x.UserId == userId);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                                                                       predicate: x => x.Memberships.User.Id == userId, 
                                                                       include: x=> x
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x=> x.User)
                                                                       .Include(x => x.Memberships)
                                                                       .ThenInclude(x => x.DigitalPlatform));

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(predicate: x => x.UserId == userId &&
                                                                                                              x.IsPaid == true);

            var mapInstructions = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(instructions);
            var mapPayments = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(payments);
            var mapExpenses = mapper.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(expenses);

            var combinedList = mapPayments
                                 .Concat(mapExpenses)
                                 .Concat(mapInstructions)
                                 .ToList();


            return combinedList;
        }
    }
}
