using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.CreditCardQueries;
using FinanceApp.Application.Features.Results.CreditCardResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Handlers.CreditCardHandler
{
    public class GetAllCreditCardsByUserQueryHandler : BaseHandler, IRequestHandler<GetAllCreditCardsByUserQuery, IList<GetAllCreditCardsByUserQueryResult>>
    {
        private readonly AuthRules authRules;
        private readonly ICreditCardService creditCardService;

        public GetAllCreditCardsByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules, ICreditCardService creditCardService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.creditCardService = creditCardService;
        }

        public async Task<IList<GetAllCreditCardsByUserQueryResult>> Handle(GetAllCreditCardsByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await creditCardService.GetAllByUserAsync(userId);
        }
    }
}
