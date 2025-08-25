using AutoMapper;
using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Results.CreditCardResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Persistence.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreditCardRules _creditCardRules;
        private readonly IMapper mapper;

        public CreditCardService(IUnitOfWork unitOfWork, CreditCardRules creditCardRules, AuthRules authRules,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _creditCardRules = creditCardRules;
            this.mapper = mapper;
        }
        public async Task AddBalanceAsync(AddBalanceCreditCardCommand request, int userId)
        {
            var creditCard = await _unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == request.Id);
            await _creditCardRules.CreditCardNoNotFound(creditCard);
            await _creditCardRules.DoesThisCardBelongToYou(creditCard, userId);

            creditCard.Balance += request.Balance;

            await _unitOfWork.GetWriteRepository<CreditCard>().UpdateAsync(creditCard);

            await _unitOfWork.GetWriteRepository<BalanceMemory>().AddAsync(new BalanceMemory
            {
                Name = request.Name ?? "Para Yükleme",
                Amount = request.Balance,
                CreditCardId = request.Id,
                AddBalanceCategory = request.AddBalanceCategory,
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task CreateAsync(CreateCreditCardCommand request, int userId)
        {
            var cards = await _unitOfWork.GetReadRepository<CreditCard>().GetAllAsync();
            await _creditCardRules.CreditCardNoNotBeSame(cards, request.CardNo);

            var creditCard = new CreditCard
            {
                CardNo = request.CardNo,
                UserId = userId,
                ValidDate = request.ValidDate,
                Cvv = request.Cvv,
                NameOnCard = request.NameOnCard,
                Balance = 0
            };

            await _unitOfWork.GetWriteRepository<CreditCard>().AddAsync(creditCard);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IList<GetAllCreditCardsByUserQueryResult>> GetAllByUserAsync(int userId)
        {
            var creditCards = await _unitOfWork.GetReadRepository<CreditCard>().GetAllAsync(x => x.UserId == userId);
            return mapper.Map<IList<GetAllCreditCardsByUserQueryResult>>(creditCards);
        }

        public async Task RemoveAsync(int creditCardId, int userId)
        {
            var creditCard = await _unitOfWork.GetReadRepository<CreditCard>().GetAsync(x => x.Id == creditCardId);
            await _creditCardRules.CreditCardNoNotFound(creditCard);
            await _creditCardRules.DoesThisCardBelongToYou(creditCard, userId);

            await _unitOfWork.GetWriteRepository<CreditCard>().HardDeleteAsync(creditCard);
            await _unitOfWork.SaveAsync();
        }
    }
}
