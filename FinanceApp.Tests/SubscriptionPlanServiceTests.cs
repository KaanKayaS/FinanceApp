using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class SubscriptionPlanServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReadRepository<SubscriptionPlan>> _mockRepo;
        private readonly SubscriptionPlanService _service;

        public SubscriptionPlanServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IReadRepository<SubscriptionPlan>>();

            _mockUnitOfWork.Setup(uow => uow.GetReadRepository<SubscriptionPlan>())
                           .Returns(_mockRepo.Object);

            _service = new SubscriptionPlanService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetPlanPriceAsync_PlanExists_ReturnsPrice()
        {
            // Arrange
            int digitalPlatformId = 1;
            SubscriptionType planType = SubscriptionType.Monthly; // enum örnek

            var subscriptionPlan = new SubscriptionPlan
            {
                DigitalPlatformId = digitalPlatformId,
                PlanType = planType,
                Price = 99.99m
            };

            _mockRepo.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>(),null,false))
                     .ReturnsAsync(subscriptionPlan);

            // Act
            var result = await _service.GetPlanPriceAsync(digitalPlatformId, planType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(subscriptionPlan.Price, result.Price);

            _mockRepo.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>(), null, false), Times.Once);
        }

        [Fact]
        public async Task GetPlanPriceAsync_PlanDoesNotExist_ThrowsException()
        {
            // Arrange
            int digitalPlatformId = 1;
            SubscriptionType planType = SubscriptionType.Monthly;

            _mockRepo.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>(), null, false))
                     .ReturnsAsync((SubscriptionPlan)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await _service.GetPlanPriceAsync(digitalPlatformId, planType));

            _mockRepo.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<SubscriptionPlan, bool>>>(), null, false), Times.Once);
        }
    }

}
