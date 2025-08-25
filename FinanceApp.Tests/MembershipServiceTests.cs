using AutoMapper;
using FinanceApp.Application.Features.Results.MembershipResult;
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
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class MembershipServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<CreditCardRules> _mockCreditCardRules;
        private readonly Mock<MembershipRules> _mockMembershipRules;
        private readonly Mock<SubscriptionPlanRules> _mockSubscriptionPlanRules;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IReadRepository<Memberships>> _mockMembershipReadRepo;
        private readonly MembershipService _service;
        private readonly Mock<IWriteRepository<Memberships>> _mockMembershipWriteRepo;


        public MembershipServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCreditCardRules = new Mock<CreditCardRules>();
            _mockMembershipRules = new Mock<MembershipRules>();
            _mockSubscriptionPlanRules = new Mock<SubscriptionPlanRules>();
            _mockMapper = new Mock<IMapper>();
            _mockMembershipReadRepo = new Mock<IReadRepository<Memberships>>();

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Memberships>())
                           .Returns(_mockMembershipReadRepo.Object);

            _mockMembershipWriteRepo = new Mock<IWriteRepository<Memberships>>();
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Memberships>())
                           .Returns(_mockMembershipWriteRepo.Object);


            _service = new MembershipService(
                _mockUnitOfWork.Object,
                _mockCreditCardRules.Object,
                _mockMembershipRules.Object,
                _mockSubscriptionPlanRules.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task CreateMembershipAsync_ValidData_CreatesMembershipAndPaymentAndUpdatesCard()
        {
            // Arrange
            int userId = 1;
            int creditCardId = 10;
            int digitalPlatformId = 20;
            SubscriptionType subscriptionType = SubscriptionType.Monthly;
            decimal originalBalance = 1000m;

            var creditCard = new CreditCard
            {
                Id = creditCardId,
                Balance = originalBalance,
                UserId = userId
            };

            Memberships? existingMembership = null;

            var subscriptionPlan = new SubscriptionPlan
            {
                Id = 5,
                DigitalPlatformId = digitalPlatformId,
                PlanType = subscriptionType,
                Price = 200m,
                DigitalPlatform = new DigitalPlatform { Id = digitalPlatformId, Name = "Netflix" },
                Memberships = new List<Memberships>() // boş liste
            };

            // Setup GetAsync mocks
            _mockUnitOfWork.Setup(u => u.GetReadRepository<CreditCard>().GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<CreditCard, bool>>>(), null, false))
                .ReturnsAsync(creditCard);

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Memberships>().GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Memberships, bool>>>(), null, false))
                .ReturnsAsync(existingMembership);

            _mockUnitOfWork.Setup(u => u.GetReadRepository<SubscriptionPlan>().GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SubscriptionPlan, bool>>>(),null,false))
                .ReturnsAsync(subscriptionPlan);

            // Setup rule methods to succeed (do nothing)
            _mockCreditCardRules.Setup(r => r.CreditCardNoNotFound(creditCard)).Returns(Task.CompletedTask);
            _mockCreditCardRules.Setup(r => r.DoesThisCardBelongToYou(creditCard, userId)).Returns(Task.CompletedTask);
            _mockMembershipRules.Setup(r => r.AlreadyMembership(It.IsAny<Memberships>())).Returns(Task.CompletedTask);
            _mockSubscriptionPlanRules.Setup(r => r.SubscriptionPlanNotFound(subscriptionPlan)).Returns(Task.CompletedTask);

            // Setup WriteRepository mocks
            var mockMembershipWriteRepo = new Mock<IWriteRepository<Memberships>>();
            var mockPaymentWriteRepo = new Mock<IWriteRepository<Payment>>();
            var mockCreditCardWriteRepo = new Mock<IWriteRepository<CreditCard>>();

            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Memberships>()).Returns(mockMembershipWriteRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Payment>()).Returns(mockPaymentWriteRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<CreditCard>()).Returns(mockCreditCardWriteRepo.Object);

            mockMembershipWriteRepo.Setup(w => w.AddAsync(It.IsAny<Memberships>())).Returns(Task.CompletedTask);
            mockPaymentWriteRepo.Setup(w => w.AddAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);

           CreditCard updatedCard = null;

            mockCreditCardWriteRepo.Setup(w => w.UpdateAsync(It.IsAny<CreditCard>()))
              .Callback<CreditCard>(c =>
              {
                  c.Balance = c.Balance; // opsiyonel; örnek olarak
                  updatedCard = c;
             })
                  .ReturnsAsync((CreditCard c) => c);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await _service.CreateMembershipAsync(userId, creditCardId, digitalPlatformId, subscriptionType);

            // Assert
            Assert.NotNull(updatedCard);
            Assert.Equal(creditCardId, updatedCard.Id);
            Assert.Equal(originalBalance - subscriptionPlan.Price, updatedCard.Balance);

            _mockCreditCardRules.Verify(r => r.CreditCardNoNotFound(creditCard), Times.Once);
            _mockCreditCardRules.Verify(r => r.DoesThisCardBelongToYou(creditCard, userId), Times.Once);

            _mockUnitOfWork.Verify(u => u.GetReadRepository<Memberships>().GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Memberships, bool>>>(), null, false), Times.Once);

            mockMembershipWriteRepo.Verify(w => w.AddAsync(It.Is<Memberships>(m =>
                m.UserId == userId &&
                m.DigitalPlatformId == digitalPlatformId &&
                m.SubscriptionPlanId == subscriptionPlan.Id
            )), Times.Once);

            mockPaymentWriteRepo.Verify(w => w.AddAsync(It.Is<Payment>(p =>
                p.CreditCardId == creditCardId &&
                p.Amount == subscriptionPlan.Price &&
                p.Memberships != null
            )), Times.Once);

            mockCreditCardWriteRepo.Verify(w => w.UpdateAsync(It.Is<CreditCard>(c =>
                c.Id == creditCardId &&
                c.Balance == originalBalance - subscriptionPlan.Price
            )), Times.Once);

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnMappedResults()
        {
            // Arrange
            int userId = 1;
            var now = DateTime.UtcNow;

            var fixedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var memberships = new List<Memberships>
            {
                new Memberships
                {
                    Id = 1,
                    UserId = userId,
                    StartDate = fixedDate,
                    EndDate = fixedDate.AddDays(30),
                    IsDeleted = false,
                    SubscriptionPlan = new SubscriptionPlan
                    {
                        PlanType = SubscriptionType.Monthly,
                        DigitalPlatform = new DigitalPlatform
                        {
                            Name = "Netflix",
                            ImagePath = "/images/netflix.png"
                        }
                    }
                }
            };

            var expectedResults = new List<GetAllMembershipsByUserQueryResult>
            {
                new GetAllMembershipsByUserQueryResult
                {
                    DigitalPlatformId = 1,
                    StartDate = fixedDate,
                    EndDate = fixedDate.AddDays(30),
                    IsDeleted = false,
                    SubscriptionPlanName = "Monthly",
                    DigitalPlatformName = "Netflix",
                    ImagePath = "/images/netflix.png"
                }
            };
            _mockMembershipReadRepo.Setup(repo =>
                 repo.GetAllAsync(
                     It.IsAny<Expression<Func<Memberships, bool>>>(),
                     It.IsAny<Func<IQueryable<Memberships>, IIncludableQueryable<Memberships, object>>>(),
                     null,
                     false
                 )).ReturnsAsync(memberships);

            // Setup mapper
            _mockMapper.Setup(m => m.Map<IList<GetAllMembershipsByUserQueryResult>>(memberships))
                .Returns(expectedResults);

            // Act
            var result = await _service.GetAllByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Netflix", result[0].DigitalPlatformName);
            Assert.Equal("Monthly", result[0].SubscriptionPlanName);

            _mockMembershipReadRepo.Verify(repo =>
            repo.GetAllAsync(
                It.IsAny<Expression<Func<Memberships, bool>>>(),
                It.IsAny<Func<IQueryable<Memberships>, IIncludableQueryable<Memberships, object>>>(),
                null,
                false
            ), Times.Once);

            _mockMapper.Verify(m => m.Map<IList<GetAllMembershipsByUserQueryResult>>(memberships), Times.Once);
        }

        [Fact]
        public async Task GetMembershipCountByUserAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            int userId = 1;
            int expectedCount = 5;

            _mockMembershipReadRepo.Setup(repo =>
                repo.CountAsync(It.Is<Expression<Func<Memberships, bool>>>(p =>
                    p.Compile().Invoke(new Memberships { UserId = userId, IsDeleted = false })  // sahte veri 
                ))).ReturnsAsync(expectedCount);

            // Act
            var result = await _service.GetMembershipCountByUserAsync(userId);

            // Assert
            Assert.Equal(expectedCount, result);

            _mockMembershipReadRepo.Verify(repo =>
                repo.CountAsync(It.IsAny<Expression<Func<Memberships, bool>>>()), Times.Once);
        }
        [Fact]
        public async Task RemoveMembershipAsync_ValidMembership_SetsIsDeletedTrueAndSavesChanges()
        {
            // Arrange
            int userId = 1;
            int digitalPlatformId = 2;

            var membership = new Memberships
            {
                Id = 1,
                UserId = userId,
                DigitalPlatformId = digitalPlatformId,
                IsDeleted = false
            };

            _mockMembershipReadRepo
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Memberships, bool>>>(), null, false))
                .ReturnsAsync(membership);

            _mockMembershipRules
                .Setup(rules => rules.MembershipNotFound(membership))
                .Returns(Task.CompletedTask);

            _mockMembershipWriteRepo
                .Setup(repo => repo.UpdateAsync(membership))
                .ReturnsAsync(membership);

            _mockUnitOfWork
                .Setup(uow => uow.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.RemoveMembershipAsync(userId, digitalPlatformId);

            // Assert
            Assert.True(membership.IsDeleted);
            _mockMembershipReadRepo.Verify(repo =>
                repo.GetAsync(It.IsAny<Expression<Func<Memberships, bool>>>(),null,false), Times.Once);

            _mockMembershipRules.Verify(rules => rules.MembershipNotFound(membership), Times.Once);
            _mockMembershipWriteRepo.Verify(repo => repo.UpdateAsync(membership), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
 }
