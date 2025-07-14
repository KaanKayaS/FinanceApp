using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.InstructionsResults;
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

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class GetAllInstructionsByUserQueryHandler : BaseHandler, IRequestHandler<GetAllInstructionsByUserQuery, IList<GetAllInstructionsByUserQueryResult>>
    {
        private readonly AuthRules authRules;

        public GetAllInstructionsByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetAllInstructionsByUserQueryResult>> Handle(GetAllInstructionsByUserQuery request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(predicate: x => x.IsPaid == false &&
                                                                                                              x.UserId == userId);

           

            return mapper.Map<IList<GetAllInstructionsByUserQueryResult>>(instructions);
        }
    }
}
