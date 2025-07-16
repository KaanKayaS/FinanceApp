using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.InstructionQueries;
using FinanceApp.Application.Features.Results.InstructionsResults;
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

namespace FinanceApp.Application.Features.Handlers.InstructionsHandlers
{
    public class GetInstructionCountByUserQueryHandler : BaseHandler, IRequestHandler<GetInstructionCountByUserQuery, GetInstructionCountByUserQueryResult>
    {
        private readonly AuthRules authRules;

        public GetInstructionCountByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<GetInstructionCountByUserQueryResult> Handle(GetInstructionCountByUserQuery request, CancellationToken cancellationToken)
        {

            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            int totalInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => x.UserId == userId);

            int waitingInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => x.IsPaid == false && x.UserId == userId);

            int paidInstruction = await unitOfWork.GetReadRepository<Instructions>().CountAsync(x => x.IsPaid == true && x.UserId == userId);

            var instructions = await unitOfWork.GetReadRepository<Instructions>().GetAllAsync(x => x.IsPaid == false && x.UserId == userId);
            decimal totalAmount = instructions.Sum(x => x.Amount);

            var result = new GetInstructionCountByUserQueryResult
            {
                TotalInstruction = totalInstruction,
                WaitingInstruction = waitingInstruction,
                PaidInstruction = paidInstruction,
                TotalAmount = totalAmount
            };

            return result;
        }
    }
}
