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
    public class ExpenseRulesTests : BaseHandlerTests
    {
        private readonly ExpenseRules rules;

        public ExpenseRulesTests()
        {
            rules = new ExpenseRules();
        }


        [Fact]
        public async Task ExpenseNameNotMustBeSame_NameExists_ShouldThrowException()
        {
            // Arrange
            var expenses = new List<Expens>
        {
            new Expens { Name = "Market" },
            new Expens { Name = "Fatura" }
        };
            var inputName = "market"; // case-insensitive eşleşme

            // Act & Assert
            await Assert.ThrowsAsync<ExpenseNameNotMustBeSameException>(() =>
                rules.ExpenseNameNotMustBeSame(expenses, inputName));
        }

        [Fact]
        public async Task ExpenseNameNotMustBeSame_NameNotExists_ShouldNotThrow()
        {
            // Arrange
            var expenses = new List<Expens>
        {
            new Expens { Name = "Market" },
            new Expens { Name = "Fatura" }
        };
            var inputName = "Kira";

            // Act
            var ex = await Record.ExceptionAsync(() =>
                rules.ExpenseNameNotMustBeSame(expenses, inputName));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public async Task IsThisYourExpense_ExpenseBelongsToAnotherUser_ShouldThrowException()
        {
            // Arrange
            var expense = new Expens { UserId = 2 };
            int currentUserId = TestUserIdInt;

            // Act & Assert
            await Assert.ThrowsAsync<IsThisYourExpenseException>(() =>
                rules.IsThisYourExpense(expense, currentUserId));
        }

        [Fact]
        public async Task IsThisYourExpense_ExpenseBelongsToCurrentUser_ShouldNotThrow()
        {
            // Arrange
            var expense = new Expens { UserId = TestUserIdInt };
            int currentUserId = TestUserIdInt;

            // Act
            var ex = await Record.ExceptionAsync(() =>
                rules.IsThisYourExpense(expense, currentUserId));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public async Task ExpensNotFound_ExpenseIsNull_ShouldThrowException()
        {
            // Arrange
            Expens? expense = null;

            // Act & Assert
            await Assert.ThrowsAsync<ExpensNotFoundException>(() =>
                rules.ExpensNotFound(expense));
        }

        [Fact]
        public async Task ExpensNotFound_ExpenseIsNotNull_ShouldNotThrow()
        {
            // Arrange
            var expense = new Expens { Id = 1, Name = "Kira" };

            // Act
            var ex = await Record.ExceptionAsync(() =>
                rules.ExpensNotFound(expense));

            // Assert
            Assert.Null(ex);
        }

    }
}
