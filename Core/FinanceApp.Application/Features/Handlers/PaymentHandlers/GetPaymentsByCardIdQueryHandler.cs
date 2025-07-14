using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.PaymentQueries;
using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Application.Features.Results.PaymentResults;
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

namespace FinanceApp.Application.Features.Handlers.PaymentHandler
{
    public class GetPaymentsByCardIdQueryHandler : BaseHandler, IRequestHandler<GetPaymentsByCardIdQuery, IList<GetPaymentsByCardIdQueryResult>>
    {
        private readonly AuthRules authRules;
        private readonly CreditCardRules creditCardRules;

        public GetPaymentsByCardIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor
            ,AuthRules authRules, CreditCardRules creditCardRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.creditCardRules = creditCardRules;
        }

        public async Task<IList<GetPaymentsByCardIdQueryResult>> Handle(GetPaymentsByCardIdQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            CreditCard creditCard = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == request.Id);
            await creditCardRules.CreditCardNoNotFound(creditCard);
            await creditCardRules.DoesThisCardBelongToYou(creditCard, userId);


            var Payment = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(x => x.CreditCardId == request.Id, include: x => x
                                                                                         .Include(x => x.Memberships)
                                                                                         .ThenInclude(x => x.SubscriptionPlan)
                                                                                         .ThenInclude(x => x.DigitalPlatform));

            var AddBalanceMemory = await unitOfWork.GetReadRepository<BalanceMemory>().GetAllAsync(x => x.CreditCardId == request.Id);

            var mapPayment = mapper.Map<IList<GetPaymentsByCardIdQueryResult>>(Payment);
            var mapAddBalanceMemory = mapper.Map<IList<GetPaymentsByCardIdQueryResult>>(AddBalanceMemory);


            var combinedList = mapPayment
                               .Concat(mapAddBalanceMemory)
                               .ToList();


            return combinedList;
        }
    }
}
