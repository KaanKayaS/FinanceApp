using AutoMapper;
using FinanceApp.Application.Features.Results.PaymentResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<CreditCardRules> _mockCreditCardRules;
        private readonly Mock<IReadRepository<CreditCard>> _mockCreditCardRepo;
        private readonly Mock<IReadRepository<Payment>> _mockPaymentRepo;
        private readonly Mock<IReadRepository<BalanceMemory>> _mockBalanceMemoryRepo;

        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCreditCardRules = new Mock<CreditCardRules>(); 

            _mockCreditCardRepo = new Mock<IReadRepository<CreditCard>>();
            _mockPaymentRepo = new Mock<IReadRepository<Payment>>();
            _mockBalanceMemoryRepo = new Mock<IReadRepository<BalanceMemory>>();

            _mockUnitOfWork.Setup(x => x.GetReadRepository<CreditCard>())
                           .Returns(_mockCreditCardRepo.Object);

            _mockUnitOfWork.Setup(x => x.GetReadRepository<Payment>())
                           .Returns(_mockPaymentRepo.Object);

            _mockUnitOfWork.Setup(x => x.GetReadRepository<BalanceMemory>())
                           .Returns(_mockBalanceMemoryRepo.Object);

            _service = new PaymentService(_mockMapper.Object, _mockUnitOfWork.Object, _mockCreditCardRules.Object);
        }

        [Fact]
        public async Task GetPaymentsByCardIdAsync_ShouldReturnMappedPaymentsAndBalances()
        {
            // Arrange
            int cardId = 1;
            int userId = 42;

            var creditCard = new CreditCard { Id = cardId, UserId = userId };

            var payments = new List<Payment>
            {
                new Payment { Id = 1, Amount = 100, CreditCardId = cardId, PaymentDate = DateTime.UtcNow }
            };

            var balances = new List<BalanceMemory>
            {
                new BalanceMemory { Id = 1, Amount = 50, CreditCardId = cardId, CreatedDate = DateTime.UtcNow }
            };

            var mappedPayments = new List<GetPaymentsByCardIdQueryResult>
            {
                new GetPaymentsByCardIdQueryResult
                {
                    Amount = 100,
                    DigitalPlatformName = "Netflix",
                    SubscriptionPlanName = "Premium",
                    PaymentDate = DateTime.UtcNow
                }
            };

            var mappedBalances = new List<GetPaymentsByCardIdQueryResult>
            {
                new GetPaymentsByCardIdQueryResult
                {
                    Amount = 50,
                    DigitalPlatformName = "Netflix",
                    SubscriptionPlanName = null,
                    PaymentDate = DateTime.UtcNow
                }
            };

            _mockCreditCardRepo.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<CreditCard, bool>>>(),null,false))
                               .ReturnsAsync(creditCard);

            _mockCreditCardRules.Setup(rules => rules.CreditCardNoNotFound(It.IsAny<CreditCard>()))
                                .Returns(Task.CompletedTask);

            _mockCreditCardRules.Setup(rules => rules.DoesThisCardBelongToYou(It.IsAny<CreditCard>(), userId))
                                .Returns(Task.CompletedTask);

            _mockPaymentRepo.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Payment, bool>>>(),
                It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(),
                null,
                false
            )).ReturnsAsync(payments);

            _mockBalanceMemoryRepo.Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<BalanceMemory, bool>>>(),
                    null,
                    null,
                    false))
                .ReturnsAsync(balances);

            _mockMapper.Setup(m => m.Map<IList<GetPaymentsByCardIdQueryResult>>(payments))
                       .Returns(mappedPayments);

            _mockMapper.Setup(m => m.Map<IList<GetPaymentsByCardIdQueryResult>>(balances))
                       .Returns(mappedBalances);

            // Act
            var result = await _service.GetPaymentsByCardIdAsync(cardId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Amount == 100);
            Assert.Contains(result, x => x.Amount == 50);

            _mockCreditCardRepo.Verify(x => x.GetAsync(It.IsAny<Expression<Func<CreditCard, bool>>>(), null, false), Times.Once);

            _mockPaymentRepo.Verify(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Payment, bool>>>(),
                It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(),
                null,
                false
            ), Times.Once);

            _mockBalanceMemoryRepo.Verify(x => x.GetAllAsync(It.IsAny<Expression<Func<BalanceMemory, bool>>>(), null, null, false), Times.Once);
            _mockMapper.Verify(x => x.Map<IList<GetPaymentsByCardIdQueryResult>>(payments), Times.Once);
            _mockMapper.Verify(x => x.Map<IList<GetPaymentsByCardIdQueryResult>>(balances), Times.Once);
        }
    }
}
