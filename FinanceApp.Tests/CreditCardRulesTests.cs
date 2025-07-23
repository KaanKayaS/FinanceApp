using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Domain.Entities;
using FinanceApp.Tests.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class CreditCardRulesTests : BaseHandlerTests
    {
        private readonly CreditCardRules _rules;

        public CreditCardRulesTests()
        {
            _rules = new CreditCardRules();
        }

        [Fact]
        public async Task DoesThisCardBelongToYou_CardBelongsToUser_ShouldNotThrow()
        {
            // Arrange
            var card = new CreditCard
            {
                Id = 1,
                UserId = TestUserIdInt
            };

            int userId = TestUserIdInt;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _rules.DoesThisCardBelongToYou(card, userId)); // methoda çalışırken exception fırlatır mı ?
            Assert.Null(exception); // Exception fırlatılmamalı
        }

        [Fact]
        public async Task DoesThisCardBelongToYou_CardDoesNotBelongToUser_ShouldThrowAuthorizationException()
        {
            // Arrange
            var card = new CreditCard
            {
                Id = 1,
                UserId = TestUserIdInt
            };

            int wrongUserId = 99;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DoesThisCardBelongToYouException>(() =>
                _rules.DoesThisCardBelongToYou(card, wrongUserId));

            Assert.IsType<DoesThisCardBelongToYouException>(exception);
        }

        [Fact]
        public async Task CreditCardNoNotBeSame_CardNoNotInList_ShouldNotThrow()
        {
            // Arrange
            var cards = new List<CreditCard>
            {
                new CreditCard { CardNo = "1111"},
                new CreditCard { CardNo = "2222"},
                new CreditCard { CardNo = "3333"},
            };

            var newCardNo = "4444";


            var exception = await Record.ExceptionAsync(() => _rules.CreditCardNoNotBeSame(cards, newCardNo));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CreditCardNoNotBeSame_CardNoExistsInList_ShouldThrowException()
        {
            // Arrange
            var cards = new List<CreditCard>
            {
                new CreditCard { CardNo = "1234" },
                new CreditCard { CardNo = "5678" }
            };
            var duplicateCardNo = "1234";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CreditCardNoNotBeSameException>(() =>
                _rules.CreditCardNoNotBeSame(cards, duplicateCardNo));

            Assert.IsType<CreditCardNoNotBeSameException>(exception);
        }

        [Fact]
        public async Task CreditCardNotFound_CardNotNull_ShouldNotThrow()
        {
            var card = new CreditCard
            {
                Id = 1,
                Balance = 2000,
                CardNo = "2525"
            };

            CreditCard Nullcard = null;

            var exception = await Record.ExceptionAsync(() => _rules.CreditCardNoNotFound(card));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CreditCardNotFound_CardIsNull_ShouldThrowException()
        {
            CreditCard Nullcard = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CreditCardNotFoundException>(() =>
                _rules.CreditCardNoNotFound(Nullcard));

            Assert.IsType<CreditCardNotFoundException>(exception);
        }
    }
}
