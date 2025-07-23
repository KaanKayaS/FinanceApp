using FinanceApp.Application.Features.Results.MembershipResult;
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
using AutoMapper;

namespace FinanceApp.Persistence.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CreditCardRules creditCardRules;
        private readonly MembershipRules membershipRules;
        private readonly SubscriptionPlanRules subscriptionPlanRules;
        private readonly IMapper mapper;

        public MembershipService(IUnitOfWork unitOfWork, CreditCardRules creditCardRules, MembershipRules membershipRules,
            SubscriptionPlanRules subscriptionPlanRules, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.creditCardRules = creditCardRules;
            this.membershipRules = membershipRules;
            this.subscriptionPlanRules = subscriptionPlanRules;
            this.mapper = mapper;
        }
        public async Task CreateMembershipAsync(int userId, int creditCardId, int digitalPlatformId, SubscriptionType subscriptionType)
        {
            var card = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == creditCardId);
            await creditCardRules.CreditCardNoNotFound(card);
            await creditCardRules.DoesThisCardBelongToYou(card, userId);

            var existingMembership = await unitOfWork.GetReadRepository<Memberships>().GetAsync(x => x.DigitalPlatformId == digitalPlatformId && x.UserId == userId);

            if (existingMembership != null && !existingMembership.IsDeleted && existingMembership.EndDate >= DateTime.UtcNow.AddHours(3))
            {
                await membershipRules.AlreadyMembership(existingMembership);
            }
            else if (existingMembership != null)
            {
                if (existingMembership.IsDeleted && existingMembership.EndDate > DateTime.UtcNow.AddHours(3))
                    throw new Exception("Üyeliğiniz aktif görünüyor, tekrar üye olamazsınız.");

                await unitOfWork.GetWriteRepository<Memberships>().HardDeleteAsync(existingMembership);
                await unitOfWork.SaveAsync();
            }

            var plan = await unitOfWork.GetReadRepository<SubscriptionPlan>().GetAsync(x =>
                x.DigitalPlatformId == digitalPlatformId && x.PlanType == subscriptionType);
            await subscriptionPlanRules.SubscriptionPlanNotFound(plan);

            if (card.Balance < plan.Price)
                throw new Exception("Yetersiz bakiye.");

            card.Balance -= plan.Price;
            var now = DateTime.UtcNow.AddHours(3);
            var endDate = subscriptionType switch
            {
                SubscriptionType.Monthly => now.AddMonths(1),
                SubscriptionType.SixMonthly => now.AddMonths(6),
                SubscriptionType.Yearly => now.AddYears(1),
                _ => throw new Exception("Geçerli bir plan tipi bulunamadı.")
            };

            var membership = new Memberships
            {
                UserId = userId,
                DigitalPlatformId = digitalPlatformId,
                SubscriptionPlanId = plan.Id,
                StartDate = now,
                EndDate = endDate
            };

            var payment = new Payment
            {
                CreditCardId = creditCardId,
                Amount = plan.Price,
                PaymentDate = now,
                Memberships = membership
            };

            await unitOfWork.GetWriteRepository<Memberships>().AddAsync(membership);
            await unitOfWork.GetWriteRepository<Payment>().AddAsync(payment);
            await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(card);

            await unitOfWork.SaveAsync();
        }

        public  async Task<IList<GetAllMembershipsByUserQueryResult>> GetAllByUserAsync(int userId)
        {
            var memberships = await unitOfWork.GetReadRepository<Memberships>().GetAllAsync(
                   predicate: x => x.UserId == userId && x.EndDate >= DateTime.UtcNow.AddHours(3),
                   include: x => x.Include(m => m.SubscriptionPlan)
                                  .ThenInclude(sp => sp.DigitalPlatform));

            return mapper.Map<IList<GetAllMembershipsByUserQueryResult>>(memberships);
        }

        public async Task<int> GetMembershipCountByUserAsync(int userId)
        {
            return await unitOfWork.GetReadRepository<Memberships>()
                                       .CountAsync(x => x.UserId == userId && x.IsDeleted == false);
        }

        public async Task RemoveMembershipAsync(int userId, int digitalPlatformId)
        {
            var membership = await unitOfWork.GetReadRepository<Memberships>()
                                                  .GetAsync(x => x.DigitalPlatformId == digitalPlatformId && x.UserId == userId);

            await membershipRules.MembershipNotFound(membership);

            if (membership.IsDeleted)
                throw new Exception("Üyeliğiniz zaten daha önce iptal edilmiştir.");

            membership.IsDeleted = true;
            await unitOfWork.GetWriteRepository<Memberships>().UpdateAsync(membership);
            await unitOfWork.SaveAsync();
        }
    }
}
