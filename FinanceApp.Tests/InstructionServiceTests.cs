using AutoMapper;
using FinanceApp.Application.Features.Commands.InstructionsCommands;
using FinanceApp.Application.Features.Exceptions;
using FinanceApp.Application.Features.Results.InstructionsResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class InstructionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReadRepository<Instructions>> _mockReadRepository;
        private readonly Mock<IWriteRepository<Instructions>> _mockWriteRepository;
        private readonly InstructionService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<InstructionRules> _mockInstructionRules;
        private readonly Mock<AuthRules> _mockAuthRules;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IPdfReportService> _mockPdfReportService;
        private readonly Mock<IMailService> _mockMailService;
        private readonly Mock<UserManager<User>> _mockUserManager;


        public InstructionServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockReadRepository = new Mock<IReadRepository<Instructions>>();
            _mockWriteRepository = new Mock<IWriteRepository<Instructions>>();
            _mockInstructionRules = new Mock<InstructionRules>();  
            _mockMapper = new Mock<IMapper>();
            _mockAuthRules = new Mock<AuthRules>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockPdfReportService = new Mock<IPdfReportService>();
            _mockMailService = new Mock<IMailService>();


            var userStoreMock = new Mock<IUserStore<User>>();

            // UserManager<User> mock nesnesini yarat
            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object,
                null,  // IOptions<IdentityOptions> optionsAccessor
                null,  // IPasswordHasher<User> passwordHasher
                null,  // IEnumerable<IUserValidator<User>> userValidators
                null,  // IEnumerable<IPasswordValidator<User>> passwordValidators
                null,  // ILookupNormalizer keyNormalizer
                null,  // IdentityErrorDescriber errors
                null,  // IServiceProvider services
                null   // ILogger<UserManager<User>> logger
            );


            _mockUnitOfWork.Setup(u => u.GetReadRepository<Instructions>()).Returns(_mockReadRepository.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Instructions>()).Returns(_mockWriteRepository.Object);

            _service = new InstructionService(
                _mockUnitOfWork.Object,
                _mockInstructionRules.Object,
                _mockMapper.Object,
                _mockAuthRules.Object,
                _mockHttpContextAccessor.Object,
                _mockUserManager.Object,
                _mockPdfReportService.Object,
                _mockMailService.Object);
        }

        [Fact]
        public async Task CreateInstructionAsync_SingleInstruction_AddsAndSaves()
        {
            // Arrange
            var userId = 1;
            var request = new CreateInstructionCommand
            {
                Title = "Ödeme",
                Amount = 100,
                ScheduledDate = new DateTime(2025, 7, 20),
                MonthlyInstruction = false
            };

            var existingInstructions = new List<Instructions>();

            _mockReadRepository
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(existingInstructions);

            _mockWriteRepository
                .Setup(w => w.AddAsync(It.IsAny<Instructions>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.CreateInstructionAsync(request, userId);

            // Assert
            _mockWriteRepository.Verify(w => w.AddAsync(It.Is<Instructions>(i =>
                i.Title == request.Title &&
                i.Amount == request.Amount &&
                i.ScheduledDate == request.ScheduledDate &&
                i.UserId == userId
            )), Times.Once);

            _mockWriteRepository.Verify(w => w.AddRangeAsync(It.IsAny<IList<Instructions>>()), Times.Never);

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateInstructionAsync_MonthlyInstruction_AddsMultipleAndSaves()
        {
            // Arrange
            var userId = 1;
            var request = new CreateInstructionCommand
            {
                Title = "Subscription",
                Amount = 50,
                ScheduledDate = new DateTime(2025, 7, 1),
                MonthlyInstruction = true,
                InstructionTime = 3
            };

            var existingInstructions = new List<Instructions>();

            _mockReadRepository
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(existingInstructions);

            _mockWriteRepository
                .Setup(w => w.AddRangeAsync(It.IsAny<IList<Instructions>>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork
                .Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.CreateInstructionAsync(request, userId);

            // Assert
            _mockWriteRepository.Verify(w => w.AddRangeAsync(It.Is<IList<Instructions>>(list =>
                list.Count == request.InstructionTime &&
                list[0].ScheduledDate == request.ScheduledDate &&
                list[1].ScheduledDate == request.ScheduledDate.AddMonths(1) &&
                list[2].ScheduledDate == request.ScheduledDate.AddMonths(2)
            )), Times.Once);

            _mockWriteRepository.Verify(w => w.AddAsync(It.IsAny<Instructions>()), Times.Never);

            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateInstructionAsync_WhenInstructionExists_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var request = new CreateInstructionCommand
            {
                Title = "Internet Bill",
                Amount = 80,
                ScheduledDate = new DateTime(2025, 7, 20),
                MonthlyInstruction = false
            };

            var existingInstructions = new List<Instructions>
            {
                new Instructions
                {
                    Title = "Internet Bill",
                    ScheduledDate = new DateTime(2025, 7, 20),
                    UserId = userId
                }
            };

            _mockReadRepository
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(existingInstructions);

            // Act & Assert
            await Assert.ThrowsAsync<InstructionNameNotMustBeSameException>(() =>
                _service.CreateInstructionAsync(request, userId));
        }

        [Fact]
        public async Task GetAllUnpaidInstructionsByUserAsync_ReturnsMappedInstructions()
        {
            // Arrange
            int userId = 1;

            var instructions = new List<Instructions>
            {
                new Instructions
                {
                    Id = 1,
                    Title = "Test Instruction 1",
                    Amount = 100,
                    ScheduledDate = new DateTime(2025, 7, 20),
                    IsPaid = false,
                    Description = "Desc 1",
                    UserId = userId
                },
                new Instructions
                {
                    Id = 2,
                    Title = "Test Instruction 2",
                    Amount = 200,
                    ScheduledDate = new DateTime(2025, 8, 20),
                    IsPaid = false,
                    Description = "Desc 2",
                    UserId = userId
                }
            };

            var mappedResults = new List<GetAllInstructionsByUserQueryResult>
            {
                new GetAllInstructionsByUserQueryResult
                {
                    Id = 1,
                    Title = "Test Instruction 1",
                    Amount = 100,
                    ScheduledDate = new DateTime(2025, 7, 20),
                    IsPaid = false,
                    Description = "Desc 1"
                },
                new GetAllInstructionsByUserQueryResult
                {
                    Id = 2,
                    Title = "Test Instruction 2",
                    Amount = 200,
                    ScheduledDate = new DateTime(2025, 8, 20),
                    IsPaid = false,
                    Description = "Desc 2"
                }
            };

            _mockReadRepository
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(instructions);

            _mockMapper
                .Setup(m => m.Map<IList<GetAllInstructionsByUserQueryResult>>(instructions))
                .Returns(mappedResults);

            // Act
            var result = await _service.GetAllUnpaidInstructionsByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(mappedResults[0].Id, result[0].Id);
            Assert.Equal(mappedResults[0].Title, result[0].Title);
            Assert.Equal(mappedResults[0].Amount, result[0].Amount);
            Assert.Equal(mappedResults[0].ScheduledDate, result[0].ScheduledDate);
            Assert.False(result[0].IsPaid);
            Assert.Equal(mappedResults[0].Description, result[0].Description);

            _mockReadRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false), Times.Once);
            _mockMapper.Verify(m => m.Map<IList<GetAllInstructionsByUserQueryResult>>(instructions), Times.Once);
        }

        [Fact]
        public async Task GetInstructionSummaryByUserIdAsync_ReturnsCorrectSummary()
        {
            int userId = 1;

            _mockReadRepository.SetupSequence(r => r.CountAsync(It.IsAny<Expression<Func<Instructions, bool>>>()))
                .ReturnsAsync(5)  // totalInstruction
                .ReturnsAsync(2)  // waitingInstruction
                .ReturnsAsync(3); // paidInstruction

            var unpaidInstructions = new List<Instructions>
            {
                new Instructions { Amount = 100, IsPaid = false, UserId = userId },
                new Instructions { Amount = 200, IsPaid = false, UserId = userId }
            };

            _mockReadRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(unpaidInstructions);

            var result = await _service.GetInstructionSummaryByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(5, result.TotalInstruction);
            Assert.Equal(2, result.WaitingInstruction);
            Assert.Equal(3, result.PaidInstruction);
            Assert.Equal(300, result.TotalAmount);

            _mockReadRepository.Verify(r => r.CountAsync(It.IsAny<Expression<Func<Instructions, bool>>>()), Times.Exactly(3));
            _mockReadRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false), Times.Once);
        }

        [Fact]
        public async Task RemoveInstructionAsync_ValidInstruction_DeletesAndSaves()
        {
            // Arrange
            int instructionId = 1;
            int userId = 2;

            var instruction = new Instructions
            {
                Id = instructionId,
                UserId = userId
            };

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(),null,false))
                .ReturnsAsync(instruction);

            _mockInstructionRules.Setup(r => r.InstructionsNotFound(instruction))
                .Returns(Task.CompletedTask);

            _mockInstructionRules.Setup(r => r.IsThisYourInstruction(instruction, userId))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(w => w.HardDeleteAsync(instruction))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.RemoveInstructionAsync(instructionId, userId);

            // Assert
            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(),null,false), Times.Once);
            _mockInstructionRules.Verify(r => r.InstructionsNotFound(instruction), Times.Once);
            _mockInstructionRules.Verify(r => r.IsThisYourInstruction(instruction, userId), Times.Once);
            _mockWriteRepository.Verify(w => w.HardDeleteAsync(instruction), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task SetInstructionPaidTrueAsync_ValidInstruction_SetsIsPaidTrueAndSaves()
        {
            // Arrange
            int instructionId = 1;
            int userId = 2;

            var instruction = new Instructions
            {
                Id = instructionId,
                UserId = userId,
                IsPaid = false
            };

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(),null,false))
                .ReturnsAsync(instruction);

            _mockInstructionRules.Setup(r => r.InstructionsNotFound(instruction))
                .Returns(Task.CompletedTask);

            _mockInstructionRules.Setup(r => r.IsThisYourInstruction(instruction, userId))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(w => w.UpdateAsync(instruction))
                .ReturnsAsync(instruction);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _service.SetInstructionPaidTrueAsync(instructionId, userId);

            // Assert
            Assert.True(result);
            Assert.True(instruction.IsPaid);

            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(), null, false), Times.Once);
            _mockInstructionRules.Verify(r => r.InstructionsNotFound(instruction), Times.Once);
            _mockInstructionRules.Verify(r => r.IsThisYourInstruction(instruction, userId), Times.Once);
            _mockWriteRepository.Verify(w => w.UpdateAsync(instruction), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateInstructionAsync_ValidRequest_UpdatesInstructionAndSaves()
        {
            // Arrange
            var userId = 1;
            var request = new UpdateInstructionCommand
            {
                Id = 10,
                Title = "Updated Title",
                Amount = 200,
                ScheduledDate = new DateTime(2025, 8, 15),
                Description = "Updated Description"
            };

            var instruction = new Instructions
            {
                Id = request.Id,
                UserId = userId,
                Title = "Old Title",
                Amount = 100,
                ScheduledDate = new DateTime(2025, 7, 10),
                Description = "Old Description"
            };

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(),null,false))
                .ReturnsAsync(instruction);

            _mockInstructionRules.Setup(r => r.InstructionsNotFound(instruction))
                .Returns(Task.CompletedTask);

            _mockInstructionRules.Setup(r => r.IsThisYourInstruction(instruction, userId))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(w => w.UpdateAsync(instruction))
                .ReturnsAsync(instruction);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.UpdateInstructionAsync(request, userId);

            // Assert
            Assert.Equal(request.Title, instruction.Title);
            Assert.Equal(request.Amount, instruction.Amount);
            Assert.Equal(request.ScheduledDate, instruction.ScheduledDate);
            Assert.Equal(request.Description, instruction.Description);

            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Instructions, bool>>>(), null, false), Times.Once);
            _mockInstructionRules.Verify(r => r.InstructionsNotFound(instruction), Times.Once);
            _mockInstructionRules.Verify(r => r.IsThisYourInstruction(instruction, userId), Times.Once);
            _mockWriteRepository.Verify(w => w.UpdateAsync(instruction), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }

}
