using FinanceApp.Application.Bases;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Application.Features.Rules
{
    public class CreditCardRules : BaseRules
    {
        public Task CreditCardNoNotBeSame(IList<CreditCard> creditCards, string cardNo)
        {

            foreach (var creditCard in creditCards) 
            {
                if(creditCard.CardNo == cardNo)
                    throw new CreditCardNoNotBeSameException();
            }
            return Task.CompletedTask;
        }

        public Task CreditCardNoNotFound(CreditCard creditCard)
        {
            if (creditCard == null) throw new CreditCardNotFoundException();
            return Task.CompletedTask;
        }

        public Task DoesThisCardBelongToYou(CreditCard creditCard, int userId)
        {
            if (creditCard.UserId != userId) throw new DoesThisCardBelongToYouException();
            return Task.CompletedTask;
        }
    }
}
