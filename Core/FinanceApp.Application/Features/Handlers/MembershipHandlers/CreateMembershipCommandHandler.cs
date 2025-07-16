using AutoMapper;
using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Commands.MembershipsCommands;
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

namespace FinanceApp.Application.Features.Handlers.MembershipHandlers
{
    public class CreateMembershipCommandHandler : BaseHandler, IRequestHandler<CreateMembershipCommand, Unit>
    {
        private readonly AuthRules authRules;
        private readonly MembershipRules membershipRules;
        private readonly SubscriptionPlanRules subscriptionPlanRules;
        private readonly CreditCardRules creditCardRules;

        public CreateMembershipCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
            AuthRules authRules, MembershipRules membershipRules,SubscriptionPlanRules subscriptionPlanRules, CreditCardRules creditCardRules) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.authRules = authRules;
            this.membershipRules = membershipRules;
            this.subscriptionPlanRules = subscriptionPlanRules;
            this.creditCardRules = creditCardRules;
        }

        public async Task<Unit> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
        {
            string? userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = await authRules.GetValidatedUserId(userIdString);

            var card = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == request.CreditCardId);
            await creditCardRules.CreditCardNoNotFound(card);

            await creditCardRules.DoesThisCardBelongToYou(card, userId);

            Memberships existingMembership = await unitOfWork.GetReadRepository<Memberships>().GetAsync(x => x.DigitalPlatformId == request.DigitalPlatformId && x.UserId == userId);

            if (existingMembership != null && !existingMembership.IsDeleted && existingMembership.EndDate >= DateTime.UtcNow.AddHours(3))
            {
                await membershipRules.AlreadyMembership(existingMembership);
            }
            else if (existingMembership != null && existingMembership.IsDeleted)
            {
                if (existingMembership.EndDate > DateTime.UtcNow.AddHours(3))
                    throw new Exception("Üyeliğinizi iptal etmiş olsanız da son kullanma tarihine kadar kullanabilirsiniz, tekrar üye olmanıza gerek yok.");
               
                    await unitOfWork.GetWriteRepository<Memberships>().HardDeleteAsync(existingMembership);
                    await unitOfWork.SaveAsync();               
            }

            else if (existingMembership != null && !existingMembership.IsDeleted && existingMembership.EndDate <= DateTime.UtcNow.AddHours(3))
            {
                await unitOfWork.GetWriteRepository<Memberships>().HardDeleteAsync(existingMembership);
                await unitOfWork.SaveAsync();
            }

            var plan = await unitOfWork.GetReadRepository<SubscriptionPlan>().GetAsync(x => x.DigitalPlatformId == request.DigitalPlatformId
                && x.PlanType == request.SubscriptionType);

            await subscriptionPlanRules.SubscriptionPlanNotFound(plan);
           

            if (card.Balance < plan.Price)
                throw new Exception("Yetersiz bakiye.");

            card.Balance -= plan.Price;

            var now = DateTime.UtcNow.AddHours(3);
            var endDate = now;

            if (plan.PlanType == SubscriptionType.Monthly)
                endDate = now.AddMonths(1);
            else if (plan.PlanType == SubscriptionType.Yearly)
                endDate = now.AddYears(1);
            else if (plan.PlanType == SubscriptionType.SixMonthly)
                endDate = now.AddMonths(6);
            else
                throw new Exception("Geçerli bir plan tipi bulunamadı.");

            var membership = new Memberships
            {
                UserId = userId,
                DigitalPlatformId = request.DigitalPlatformId,
                SubscriptionPlanId = plan.Id,
                StartDate = now,
                EndDate = endDate
            };

            // Payment kaydı (membership henüz DB'de yok, ama burada navigation olarak bağlayabiliriz)
            var payment = new Payment
            {
                CreditCardId = request.CreditCardId,
                Amount = plan.Price,
                PaymentDate = now,
                Memberships = membership
            };

            try
            {
                // Hem üyelik hem ödeme aynı anda ekleniyor
                await unitOfWork.GetWriteRepository<Memberships>().AddAsync(membership);
                await unitOfWork.GetWriteRepository<Payment>().AddAsync(payment);
                await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(card);

                // Tüm değişiklikler tek transaction’da commit
                await unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("İşlem sırasında bir hata oluştu, ödeme alınamadı veya üyelik oluşturulamadı.", ex);
            }

            return Unit.Value;
        }
    }
}





