using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.CreditCardCommands;
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
    public class RemoveCreditCardCommandHandler : BaseHandler, IRequestHandler<RemoveCreditCardCommand,Unit>
    {
        private readonly ICreditCardService creditCardService;
        private readonly AuthRules authRules;

        public RemoveCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger,
            ICreditCardService creditCardService, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.creditCardService = creditCardService;
            this.authRules = authRules;
        }

        public async Task<Unit> Handle(RemoveCreditCardCommand request, CancellationToken cancellationToken)
        {

            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await creditCardService.RemoveAsync(request.Id, userId);
            return Unit.Value;
        }
    }
}
