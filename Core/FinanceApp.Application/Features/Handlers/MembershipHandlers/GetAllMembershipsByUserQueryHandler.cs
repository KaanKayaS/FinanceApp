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
    public class GetAllMembershipsByUserQueryHandler : BaseHandler, IRequestHandler<GetAllMembershipsByUserQuery, IList<GetAllMembershipsByUserQueryResult>>
    {
        private readonly AuthRules authRules;

        public GetAllMembershipsByUserQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
        }

        public async Task<IList<GetAllMembershipsByUserQueryResult>> Handle(GetAllMembershipsByUserQuery request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var values = await unitOfWork.GetReadRepository<Memberships>().GetAllAsync(x => x.UserId == userId && x.IsDeleted == false, include: x => x
                                                                                         .Include(x => x.SubscriptionPlan)
                                                                                         .ThenInclude(x => x.DigitalPlatform));
                                                                                


            return mapper.Map<IList<GetAllMembershipsByUserQueryResult>>(values);


        }
    }
}
