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
    public class CreateCreditCardCommandHandler : BaseHandler, IRequestHandler<CreateCreditCardCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly CreditCardRules creditCardRules;
        public CreateCreditCardCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor
            ,AuthRules authRules, CreditCardRules creditCardRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.creditCardRules = creditCardRules;
        }

        public async Task<Unit> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            IList<CreditCard> Cards = await unitOfWork.GetReadRepository<CreditCard>().GetAllAsync();
            await creditCardRules.CreditCardNoNotBeSame(Cards, request.CardNo);

            await unitOfWork.GetWriteRepository<CreditCard>().AddAsync(new CreditCard
            {
                CardNo = request.CardNo,
                UserId = userId,
                ValidDate = request.ValidDate,
                Cvv = request.Cvv,
                NameOnCard = request.NameOnCard,
                Balance = 2000.00m,
            });

            await unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
