using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.MembershipsCommands;
using FinanceApp.Application.Features.Handlers.CreditCardHandler;
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

namespace FinanceApp.Application.Features.Handlers.MembershipHandlers
{
    public class CreateMembershipCommandHandler : BaseHandler, IRequestHandler<CreateMembershipCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly IMembershipService membershipService;

        public CreateMembershipCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            AuthRules authRules,IMembershipService membershipService) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.membershipService = membershipService;
        }

        public async Task<Unit> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await membershipService.CreateMembershipAsync(userId, request.CreditCardId, request.DigitalPlatformId, request.SubscriptionType);

            return Unit.Value;
        }
    }
}





