using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
using FinanceApp.Application.Features.Queries.PaymentQueries;
using FinanceApp.Application.Features.Results.MembershipResult;
using FinanceApp.Application.Features.Results.PaymentResults;
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

namespace FinanceApp.Application.Features.Handlers.PaymentHandler
{
    public class GetPaymentsByCardIdQueryHandler : BaseHandler, IRequestHandler<GetPaymentsByCardIdQuery, IList<GetPaymentsByCardIdQueryResult>>
    {
        private readonly AuthRules authRules;
        private readonly IPaymentService paymentService;

        public GetPaymentsByCardIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor , ILogger<AddBalanceCreditCardCommandHandler> logger
            ,AuthRules authRules, IPaymentService paymentService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.paymentService = paymentService;
        }

        public async Task<IList<GetPaymentsByCardIdQueryResult>> Handle(GetPaymentsByCardIdQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await paymentService.GetPaymentsByCardIdAsync(request.Id, userId);
        }
    }
}
