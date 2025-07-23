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
    public class CreateCreditCardCommandHandler : BaseHandler, IRequestHandler<CreateCreditCardCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly ICreditCardService creditCardService;

        public CreateCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<AddBalanceCreditCardCommandHandler> logger
            , AuthRules authRules,ICreditCardService creditCardService) : base(mapper, unitOfWork, httpContextAccessor , logger)
        {
            this.authRules = authRules;
            this.creditCardService = creditCardService;
        }

        public async Task<Unit> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await creditCardService.CreateAsync(request, userId);
            
            return Unit.Value;
        }
    }
}
