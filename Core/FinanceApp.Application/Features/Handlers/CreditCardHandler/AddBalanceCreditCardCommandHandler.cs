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
    public class AddBalanceCreditCardCommandHandler : BaseHandler, IRequestHandler<AddBalanceCreditCardCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly CreditCardRules creditCardRules;

        public AddBalanceCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor
            , AuthRules authRules,CreditCardRules creditCardRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.creditCardRules = creditCardRules;
        }

        public async Task<Unit> Handle(AddBalanceCreditCardCommand request, CancellationToken cancellationToken)
        {
            CreditCard creditCard = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == request.Id);
            await creditCardRules.CreditCardNoNotFound(creditCard);

            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            await creditCardRules.DoesThisCardBelongToYou(creditCard, userId);

            creditCard.Balance += request.Balance;
            

            await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(creditCard);

            await unitOfWork.GetWriteRepository<BalanceMemory>().AddAsync(new BalanceMemory
            {
                Name = "Para Yükleme",
                Amount = request.Balance,
                CreditCardId = request.Id,
            });

            await unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
