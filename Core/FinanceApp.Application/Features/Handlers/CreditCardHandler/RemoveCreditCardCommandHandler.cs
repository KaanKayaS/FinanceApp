using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.CreditCardCommands;
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
    public class RemoveCreditCardCommandHandler : BaseHandler, IRequestHandler<RemoveCreditCardCommand,Unit>
    {
        private readonly CreditCardRules creditCardRules;
        private readonly AuthRules authRules;

        public RemoveCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            CreditCardRules creditCardRules, AuthRules authRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.creditCardRules = creditCardRules;
            this.authRules = authRules;
        }

        public async Task<Unit> Handle(RemoveCreditCardCommand request, CancellationToken cancellationToken)
        {
            CreditCard creditCard = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == request.Id);
            await creditCardRules.CreditCardNoNotFound(creditCard);

            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            await creditCardRules.DoesThisCardBelongToYou(creditCard, userId);

            await unitOfWork.GetWriteRepository<CreditCard>().HardDeleteAsync(creditCard);

            await unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
