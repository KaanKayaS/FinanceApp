using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.SubscriptionPlansQueries;
using FinanceApp.Application.Features.Results.SubscriptionPlansResults;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.SubscriptionPlansHandlers
{
    public class GetAllSubscriptionPlansQueryHandler : BaseHandler, IRequestHandler<GetAllSubscriptionPlansQuery, GetAllSubscriptionPlansQueryResult>
    {
        public GetAllSubscriptionPlansQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
        }

        public async Task<GetAllSubscriptionPlansQueryResult> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            var subPlan = await unitOfWork.GetReadRepository<SubscriptionPlan>().GetAsync(x => x.DigitalPlatformId == request.DigitalPlatformId &&
                                                                                            x.PlanType == request.PlanType);

            if (subPlan == null)
                throw new Exception("Ödeme planı bulunamadı");

            var price = new GetAllSubscriptionPlansQueryResult
            {
                Price = subPlan.Price,
            };

            return price;
        }
    }
}
