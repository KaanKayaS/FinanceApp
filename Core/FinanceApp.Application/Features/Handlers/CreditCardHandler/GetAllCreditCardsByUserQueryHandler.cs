using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.CreditCardQueries;
using FinanceApp.Application.Features.Results.CreditCardResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
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

        public GetAllCreditCardsByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetAllCreditCardsByUserQueryResult>> Handle(GetAllCreditCardsByUserQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var values = await unitOfWork.GetReadRepository<CreditCard>().GetAllAsync(x=>x.UserId == userId);

            return mapper.Map<IList<GetAllCreditCardsByUserQueryResult>>(values);
        }
    }
}
