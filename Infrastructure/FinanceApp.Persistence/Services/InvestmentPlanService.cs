using AutoMapper;
using Azure.Core;
using FinanceApp.Application.Features.Commands.InvestmentPlanCommands;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Results.InvestmentPlanResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinanceApp.Persistence.Services
{
    public class InvestmentPlanService : IInvestmentPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly CreditCardRules creditCardRules;
        private readonly InvestmentPlanRules investmentPlanRules;
        private readonly CreditCardRules cardRules;

        public InvestmentPlanService(IUnitOfWork unitOfWork, IMapper mapper, CreditCardRules creditCardRules, InvestmentPlanRules investmentPlanRules,
            CreditCardRules cardRules)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.creditCardRules = creditCardRules;
            this.investmentPlanRules = investmentPlanRules;
            this.cardRules = cardRules;
        }

        public async Task AddBalancePlan(AddBalanceInvestmentPlanCommand command, int userId)
        {
            var plan = await unitOfWork.GetReadRepository<InvestmentPlan>().GetAsync(x => x.Id == command.InvestmentPlanId);
            await investmentPlanRules.PlanNotFound(plan);
            await investmentPlanRules.DoesThisPlanBelongToYou(plan,userId);

            var card = await unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == command.CardId);
            await creditCardRules.CreditCardNoNotFound(card);
            await creditCardRules.DoesThisCardBelongToYou(card, userId);

            if (card.Balance < command.Price)
                throw new Exception("Kart bakiyeniz yetersiz");

            card.Balance -= command.Price;
            plan.CurrentAmount += command.Price;

            if(plan.CurrentAmount >= plan.TargetPrice)
            {
                plan.IsCompleted = true;
            }


            await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(card);
            await unitOfWork.GetWriteRepository<InvestmentPlan>().UpdateAsync(plan);


            await unitOfWork.GetWriteRepository<InvestmentPlanPayment>().AddAsync(new InvestmentPlanPayment
            {
               Price = command.Price,
               CreditCardId = card.Id,
               InvestmentPlanId = plan.Id,  
            });

            // kart hesap harektlerinde gözüksün 
            await unitOfWork.GetWriteRepository<BalanceMemory>().AddAsync(new BalanceMemory
            {
               Name = "Kumbaraya Para ekleme",
               CreditCardId = card.Id,
               Amount = command.Price, 
               AddBalanceCategory = AddBalanceCategory.PiggyBank
            });

            await unitOfWork.SaveAsync();

        }

        public async Task CreateAsync(CreateInvestmentPlanCommand command, int userId)
        {
            var investmentPlan = new InvestmentPlan
            {
                Name = command.Name,
                Description = command.Description,
                TargetPrice = command.TargetPrice,
                TargetDate = command.TargetDate,
                UserId = userId,
                InvestmentCategory = command.InvestmentCategory,
                InvestmentFrequency = command.InvestmentFrequency,
            };

            await unitOfWork.GetWriteRepository<InvestmentPlan>().AddAsync(investmentPlan);
            await unitOfWork.SaveAsync();

            
        }

        public async Task<IList<GetAllInvestmenPlanByUserQueryResult>> GetAllPlanByUserAsync(int userId)
        {
            var plans = await unitOfWork.GetReadRepository<InvestmentPlan>().GetAllAsync(x => x.UserId == userId);

            var resultList = new List<GetAllInvestmenPlanByUserQueryResult>();

            foreach (var investmentPlan in plans) 
            { 

                var totalDays = (investmentPlan.TargetDate - investmentPlan.CreatedDate).Days;

                int paymentCount = investmentPlan.InvestmentFrequency switch
                {
                    InvestmentFrequency.Daily => totalDays,
                    InvestmentFrequency.Weekly => totalDays / 7,
                    InvestmentFrequency.Monthly => ((investmentPlan.TargetDate.Year - investmentPlan.CreatedDate.Year) * 12) + investmentPlan.TargetDate.Month - investmentPlan.CreatedDate.Month,
                    _ => 0
                };

                // Ödeme başına düşen fiyat
                decimal perPaymentAmount = paymentCount > 0 ? Math.Round(investmentPlan.TargetPrice / paymentCount, 2) : investmentPlan.TargetPrice;

                var howManyDaysLeft = Math.Max(0, (investmentPlan.TargetDate - DateTime.UtcNow.AddHours(3)).Days);

                var mapped = new GetAllInvestmenPlanByUserQueryResult
                {
                    Id = investmentPlan.Id,
                    Name = investmentPlan.Name,
                    Description = investmentPlan.Description,
                    TargetPrice = investmentPlan.TargetPrice,
                    CurrentAmount = investmentPlan.CurrentAmount,
                    TargetDate = investmentPlan.TargetDate,
                    IsCompleted = investmentPlan.IsCompleted,
                    InvestmentCategory = investmentPlan.InvestmentCategory,
                    InvestmentFrequency = investmentPlan.InvestmentFrequency,
                    PerPaymentAmount = perPaymentAmount,
                    HowManyDaysLeft = howManyDaysLeft
                };

                resultList.Add(mapped);


            }

            return resultList;
        }
        public async Task RemoveInvestmentAsync(int investmentId, int userId)
        {
            var plan = await unitOfWork.GetReadRepository<InvestmentPlan>().GetAsync(x => x.Id == investmentId);
            await investmentPlanRules.PlanNotFound(plan);
            await investmentPlanRules.DoesThisPlanBelongToYou(plan, userId);

            var payments = await unitOfWork.GetReadRepository<InvestmentPlanPayment>().GetAllAsync(x => x.InvestmentPlanId == plan.Id);

            var cardIds = payments.Select(x => x.CreditCardId).Distinct().ToList();
            var cards = await unitOfWork.GetReadRepository<CreditCard>()
                                        .GetAllAsync(x => cardIds.Contains(x.Id));

            foreach (var payment in payments) 
            {
                var card = cards.First(x => x.Id == payment.CreditCardId);
                await creditCardRules.CreditCardNoNotFound(card);
                await creditCardRules.DoesThisCardBelongToYou(card, userId);

                card.Balance += payment.Price;
                await unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(card);

                await unitOfWork.GetWriteRepository<BalanceMemory>().AddAsync(new BalanceMemory
                {
                    Name = "Kumbara Kırma",
                    CreditCardId = card.Id,
                    Amount = payment.Price,
                    AddBalanceCategory = AddBalanceCategory.CrashPigyBank,
                });

            }

            await unitOfWork.GetWriteRepository<InvestmentPlan>().HardDeleteAsync(plan);
            await unitOfWork.SaveAsync();
        }
    }
}
