using FinanceApp.Application.Interfaces.Hangfire;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Hangfire
{
    public class MembershipRenewalService : IMembershipRenewalService
    {
        private readonly IUnitOfWork unitOfWork;

        public MembershipRenewalService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task RenewMembershipsAsync()
        {
            var now = DateTime.UtcNow.AddHours(3);

            var membershipsToRenew = await unitOfWork.GetReadRepository<Memberships>().GetAllAsync(
                x => x.IsAutoRenewal && !x.IsDeleted && x.EndDate <= now,
                 include: q => q.Include(m => m.SubscriptionPlan)
            );

            foreach (var membership in membershipsToRenew)
            {
                var price = membership.SubscriptionPlan.Price;
                var planType = membership.SubscriptionPlan.PlanType;

                var card = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(c => c.UserId == membership.UserId);

                membership.SubscriptionPlan = null;

                if (card == null || card.Balance < price)
                    continue; 
        

                card.Balance -= price;

                // Bitiş tarihini uzat
                membership.StartDate = now;
                membership.EndDate = planType switch
                {
                    SubscriptionType.Monthly => now.AddMonths(1),
                    SubscriptionType.SixMonthly => now.AddMonths(6),
                    SubscriptionType.Yearly => now.AddYears(1),
                    _ => throw new Exception("Plan tipi bilinmiyor.")
                };


                // Ödeme kaydı oluştur
                await unitOfWork.GetWriteRepository<Payment>().AddAsync(new Payment
                {
                    CreditCardId = card.Id,
                    Amount = price,
                    PaymentDate = now,
                    Memberships = membership,
                });

                await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(card);
                await unitOfWork.GetWriteRepository<Memberships>().UpdateAsync(membership);
            }

            await unitOfWork.SaveAsync();
        }
    }
}
