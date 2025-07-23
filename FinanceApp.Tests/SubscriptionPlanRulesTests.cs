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
    public class SubscriptionPlanRulesTests : BaseHandlerTests
    {
        private readonly SubscriptionPlanRules _rules;

        public SubscriptionPlanRulesTests()
        {
            _rules = new SubscriptionPlanRules();
        }

        [Fact]
        public async Task SubscriptionPlanNotFound_WhenPlanIsNull_ShouldThrowException()
        {
            // Arrange
            SubscriptionPlan? plan = null;

            // Act & Assert
            await Assert.ThrowsAsync<SubscriptionPlanNotFoundException>(() =>
                _rules.SubscriptionPlanNotFound(plan));
        }

        [Fact]
        public async Task SubscriptionPlanNotFound_WhenPlanExists_ShouldNotThrowException()
        {
            // Arrange
            var plan = new SubscriptionPlan
            {
                Id = 1,
                PlanType = SubscriptionType.Yearly,
                Price = 29.99m,
            };

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _rules.SubscriptionPlanNotFound(plan));

            // Assert
            Assert.Null(exception);
        }
    }
}
