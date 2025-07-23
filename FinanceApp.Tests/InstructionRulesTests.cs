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
    public class InstructionRulesTests : BaseHandlerTests
    {
        private readonly InstructionRules rules;

        public InstructionRulesTests()
        {
            rules = new InstructionRules();
        }
        [Fact]
        public async Task InstructionNameNotMustBeSame_ExistingTitleAndDate_ShouldThrowException()
        {
            // Arrange
            var scheduledDate = DateTime.UtcNow;
            var instructions = new List<Instructions>
        {
            new Instructions
            {
                Title = "Elektrik",
                ScheduledDate = scheduledDate,
                CreatedDate = DateTime.UtcNow
            }
        };

            var inputTitle = "elektrik"; // case-insensitive eşleşme

            // Act & Assert
            await Assert.ThrowsAsync<InstructionNameNotMustBeSameException>(() =>
                rules.InstructionNameNotMustBeSame(instructions, inputTitle, scheduledDate));
        }

        [Fact]
        public async Task InstructionNameNotMustBeSame_DifferentTitleOrDate_ShouldNotThrow()
        {
            // Arrange
            var scheduledDate = DateTime.UtcNow;
            var instructions = new List<Instructions>
        {
            new Instructions
            {
                Title = "Su",
                ScheduledDate = scheduledDate,
                CreatedDate = DateTime.UtcNow
            }
        };

            var inputTitle = "Elektrik";

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.InstructionNameNotMustBeSame(instructions, inputTitle, scheduledDate));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task InstructionsNotFound_InstructionIsNull_ShouldThrowException()
        {
            // Arrange
            Instructions? instruction = null;

            // Act & Assert
            await Assert.ThrowsAsync<InstructionsNotFoundException>(() =>
                rules.InstructionsNotFound(instruction));
        }

        [Fact]
        public async Task InstructionsNotFound_InstructionIsNotNull_ShouldNotThrow()
        {
            // Arrange
            var instruction = new Instructions
            {
                Id = 1,
                Title = "Kira"
            };

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.InstructionsNotFound(instruction));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task IsThisYourInstruction_UserIdMismatch_ShouldThrowException()
        {
            // Arrange
            var instruction = new Instructions
            {
                UserId = TestUserIdInt
            };
            int currentUserId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<IsThisYourInstructionException>(() =>
                rules.IsThisYourInstruction(instruction, currentUserId));
        }

        [Fact]
        public async Task IsThisYourInstruction_UserIdMatch_ShouldNotThrow()
        {
            // Arrange
            var instruction = new Instructions
            {
                UserId = TestUserIdInt
            };
            int currentUserId = TestUserIdInt;

            // Act
            var exception = await Record.ExceptionAsync(() =>
                rules.IsThisYourInstruction(instruction, currentUserId));

            // Assert
            Assert.Null(exception);
        }
    }
}
