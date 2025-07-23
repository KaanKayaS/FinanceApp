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
    public class MembershipRulesTests : BaseHandlerTests
    {
        private readonly MembershipRules rules;

        public MembershipRulesTests()
        {
            rules = new MembershipRules();
        }

        [Fact]
        public async Task AlreadyMembership_MembershipExists_ShouldThrowException()
        {
            // Arrange
            var membership = new Memberships
            {
                Id = 1,
                UserId = TestUserIdInt
            };

            // Act & Assert
            await Assert.ThrowsAsync<AlreadyMembershipException>(() =>
                rules.AlreadyMembership(membership));
        }

        [Fact]
        public async Task AlreadyMembership_MembershipIsNull_ShouldNotThrow()
        {
            // Arrange
            Memberships? membership = null;

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.AlreadyMembership(membership));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task MembershipNotFound_MembershipIsNull_ShouldThrowException()
        {
            // Arrange
            Memberships? membership = null;

            // Act & Assert
            await Assert.ThrowsAsync<MembershipNotFoundException>(() =>
                rules.MembershipNotFound(membership));
        }

        [Fact]
        public async Task MembershipNotFound_MembershipExists_ShouldNotThrow()
        {
            // Arrange
            var membership = new Memberships
            {
                Id = 1,
                UserId = TestUserIdInt
            };

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.MembershipNotFound(membership));

            // Assert
            Assert.Null(exception);
        }
    
}
}
