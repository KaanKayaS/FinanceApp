using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Queries.MembershipsQueries;
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

namespace FinanceApp.Application.Features.Handlers.MembershipHandlers
{
    public class GetMembershipCountByUserQueryHandler : BaseHandler, IRequestHandler<GetMembershipCountByUserQuery, int>
    {
        private readonly AuthRules authRules;

        public GetMembershipCountByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<int> Handle(GetMembershipCountByUserQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            int count = await unitOfWork.GetReadRepository<Memberships>().CountAsync(x => x.UserId == userId && x.IsDeleted == false);

            return count;
        }
    }
}
