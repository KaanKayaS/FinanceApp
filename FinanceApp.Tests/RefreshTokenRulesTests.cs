using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Tests.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class RefreshTokenRulesTests : BaseHandlerTests
    {
        private readonly RefreshTokenRules rules;

        public RefreshTokenRulesTests()
        {
            rules = new RefreshTokenRules();
        }

        [Fact]
        public async Task RefreshTokenShouldNotBeExpired_ExpiryDateInPast_ShouldThrowException()
        {
            // Arrange
            DateTime expiryDate = DateTime.UtcNow.AddMinutes(-1); // süresi geçmiş

            // Act & Assert
            await Assert.ThrowsAsync<RefreshTokenShouldNotBeExpiredException>(() =>
                rules.RefreshTokenShouldNotBeExpired(expiryDate));
        }

        [Fact]
        public async Task RefreshTokenShouldNotBeExpired_ExpiryDateInFuture_ShouldNotThrow()
        {
            // Arrange
            DateTime expiryDate = DateTime.UtcNow.AddMinutes(5); // süresi henüz dolmamış

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.RefreshTokenShouldNotBeExpired(expiryDate));

            // Assert
            Assert.Null(exception);
        }
    }
}
