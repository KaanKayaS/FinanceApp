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
    public class AddBalanceCreditCardCommandHandler : BaseHandler, IRequestHandler<AddBalanceCreditCardCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly ICreditCardService creditCardService;
        private readonly ILogger<AddBalanceCreditCardCommandHandler> logger;

        public AddBalanceCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor
            , AuthRules authRules,ICreditCardService creditCardService, ILogger<AddBalanceCreditCardCommandHandler> logger) : base(mapper, unitOfWork, httpContextAccessor, logger)
        {
            this.authRules = authRules;
            this.creditCardService = creditCardService;
            this.logger = logger;
        }

        public async Task<Unit> Handle(AddBalanceCreditCardCommand request, CancellationToken cancellationToken)
        {
            int userId = await authRules.GetValidatedUserId(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await creditCardService.AddBalanceAsync(request, userId);
            logger.LogInformation("Bakiye güncellendi");
            return Unit.Value;
        }
    }
}
