using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.MembershipsCommands;
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

namespace FinanceApp.Application.Features.Handlers.MembershipHandlers
{
    public class RemoveMembershipCommandHandler :BaseHandler, IRequestHandler<RemoveMembershipCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly MembershipRules membershipRules;

        public RemoveMembershipCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, MembershipRules membershipRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.membershipRules = membershipRules;
        }

        public async Task<Unit> Handle(RemoveMembershipCommand request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            Memberships memberships = await unitOfWork.GetReadRepository<Memberships>().GetAsync(x => x.DigitalPlatformId == request.Id && x.UserId == userId);
            await membershipRules.MembershipNotFound(memberships);

            if(memberships.IsDeleted == true)
            {
                throw new Exception("Üyeliğiniz zaten daha önce iptal edilmiştir.");
            }

            memberships.IsDeleted = true;
            await unitOfWork.GetWriteRepository<Memberships>().UpdateAsync(memberships);

            await unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
