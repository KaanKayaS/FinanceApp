using AutoMapper;
using FinanceApp.Application.Features.Results.PaymentResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Persistence.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly CreditCardRules creditCardRules;

        public PaymentService(IMapper mapper, IUnitOfWork unitOfWork, CreditCardRules creditCardRules)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.creditCardRules = creditCardRules;
        }
        public async Task<IList<GetPaymentsByCardIdQueryResult>> GetPaymentsByCardIdAsync(int cardId, int userId)
        {
            var creditCard = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == cardId);
            await creditCardRules.CreditCardNoNotFound(creditCard);
            await creditCardRules.DoesThisCardBelongToYou(creditCard, userId);

            var payments = await unitOfWork.GetReadRepository<Payment>().GetAllAsync(
                x => x.CreditCardId == cardId,
                include: x => x
                    .Include(p => p.Memberships)
                    .ThenInclude(m => m.SubscriptionPlan)
                .ThenInclude(sp => sp.DigitalPlatform)
            );

            var balanceMemory = await unitOfWork.GetReadRepository<BalanceMemory>().GetAllAsync(x => x.CreditCardId == cardId);

            var mappedPayments = mapper.Map<IList<GetPaymentsByCardIdQueryResult>>(payments);
            var mappedBalanceMemory = mapper.Map<IList<GetPaymentsByCardIdQueryResult>>(balanceMemory);

            return mappedPayments.Concat(mappedBalanceMemory).OrderByDescending(x => x.PaymentDate).ToList();
        }
    }
}
