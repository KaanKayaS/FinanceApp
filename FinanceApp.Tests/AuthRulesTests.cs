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
    public class AuthRulesTests : BaseHandlerTests
    {
        private readonly AuthRules _rules;

        public AuthRulesTests()
        {
            _rules = new AuthRules();
        }

        [Fact]
        public async Task UserShouldNotBeExist_UserIsNull_ShouldNotThrow()
        {
            // Arrange
            User? user = null;
            // Act
            var exception = await Record.ExceptionAsync(() => _rules.UserShouldNotBeExist(user));
            // Assert
            Assert.Null(exception); // Exception olmamalı
        }

        [Fact]
        public async Task UserShouldNotBeExist_UserIsNotNull_ShouldThrow()
        {
            // Arrange
            User? user = new User
            {
                Id = TestUserIdInt,
                FullName = "Kaan Kaya",
            };

            var exception = await Assert.ThrowsAsync<UserAlreadyExistException>(() =>
                 _rules.UserShouldNotBeExist(user));

            Assert.IsType<UserAlreadyExistException>(exception);
        }

        [Fact]
        public async Task EmailOrPasswordShouldNotBeInvalid_UserİsNull_ShouldThrowException()
        {
            // Arrange
            User? user = null;
            bool checkpassword = true;

            var exception = await Assert.ThrowsAsync<EmailOrPasswordShouldNotBeInvalidException>(() =>
                 _rules.EmailOrPasswordShouldNotBeInvalid(user, checkpassword));

            Assert.IsType<EmailOrPasswordShouldNotBeInvalidException>(exception);
        }

        [Fact]
        public async Task EmailOrPasswordShouldNotBeInvalid_ValidUserAndPasswordCheck_ShouldNotThrow()
        {
            // Arrange
            var user = new User { Id = 1, Email = "kaan@info" };
            bool checkpassword = true;

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _rules.EmailOrPasswordShouldNotBeInvalid(user, checkpassword));

            // Assert
            Assert.Null(exception); // Exception fırlamamalı
        }

        [Fact]
        public async Task EmailOrPasswordShouldNotBeInvalid_CheckPasswordFalse_ShouldThrowException()
        {
            // Arrange
            var user = new User { Id = 1, Email = "kaan@info" };
            bool checkpassword = false;

            // Act & Assert
            await Assert.ThrowsAsync<EmailOrPasswordShouldNotBeInvalidException>(() =>
                _rules.EmailOrPasswordShouldNotBeInvalid(user, checkpassword));
        }

        [Fact]
        public async Task EmailAddressShouldBeValid_UserIsNull_ShouldThrowException()
        {
            // Arrange
            User? user = null;
            // Act & Assert
            var exception = await Assert.ThrowsAsync<EmailAddressShouldBeValidException>(() => _rules.EmailAddressShouldBeValid(user));

        }

        [Fact]
        public async Task EmailAddressShouldBeValid_UserIsNotNull_ShouldNotThrow()
        {
            // Arrange
            var user = new User { Id = TestUserIdInt}; // Gerekirse gerekli özellikleri doldur

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _rules.EmailAddressShouldBeValid(user));

            Assert.Null(exception); // Exception fırlatmamalı
        }

        [Fact]
        public async Task GetValidatedUserId_InvalidString_ShouldThrowException()
        {
            // Arrange
            var invalidUserId = "abc123"; // int'e çevrilemez

            // Act & Assert
            await Assert.ThrowsAsync<GetValidatedUserIdException>(() => _rules.GetValidatedUserId(invalidUserId));
        }

        [Fact]
        public async Task GetValidatedUserId_ValidString_ShouldReturnParsedInt()
        {
            // Arrange
            var validUserId = TestUserId;
            // Act
            var result = await _rules.GetValidatedUserId(validUserId);

            // Assert
            Assert.Equal(TestUserIdInt, result);
        }

        [Fact]
        public async Task UserNameMustBeUnique_FullNameExistsInList_ShouldThrowException()
        {
            // Arrange
            var userList = new List<User>
        {
            new User { FullName = "Kaan Kaya" },
            new User { FullName = "Beyza Nur" }
        };

            var inputFullName = "kaan kaya"; // Case-insensitive eşleşme

            // Act & Assert
            await Assert.ThrowsAsync<UserNameMustBeUniqueException>(() =>
                _rules.UserNameMustBeUnique(inputFullName, userList));
        }

        [Fact]
        public async Task UserNameMustBeUnique_FullNameNotInList_ShouldNotThrowException()
        {
            // Arrange
            var userList = new List<User>
        {
            new User { FullName = "Kaan Kaya" },
            new User { FullName = "Beyza Nur" }
        };

            var inputFullName = "mehmet can";

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _rules.UserNameMustBeUnique(inputFullName, userList));

            // Assert
            Assert.Null(exception);
        }
    }
}
