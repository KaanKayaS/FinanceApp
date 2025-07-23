using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.SubscriptionPlansQueries;
using FinanceApp.Application.Features.Results.SubscriptionPlansResults;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.SubscriptionPlansHandlers
{
    public class GetAllSubscriptionPlansQueryHandler : BaseHandler, IRequestHandler<GetAllSubscriptionPlansQuery, GetAllSubscriptionPlansQueryResult>
    {
        private readonly ISubscriptionPlanService subscriptionPlanService;

        public GetAllSubscriptionPlansQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            ISubscriptionPlanService subscriptionPlanService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.subscriptionPlanService = subscriptionPlanService;
        }

        public async Task<GetAllSubscriptionPlansQueryResult> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
        {
            return await subscriptionPlanService.GetPlanPriceAsync(request.DigitalPlatformId, request.PlanType);
        }
    }
}
