using AutoMapper;
using FinanceApp.Application.Features.Commands.ExpenseCommands;
using FinanceApp.Application.Features.Results.ExpenseResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using Microsoft.AspNetCore.Identity;
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
    public class ExpenseServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReadRepository<Expens>> _mockReadRepository;
        private readonly Mock<IWriteRepository<Expens>> _mockWriteRepository;
        private readonly Mock<ExpenseRules> _mockExpenseRules;
        private readonly ExpenseService _service;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly Mock<ICreditCardService> _mockCreditCardService;
        private readonly Mock<IPdfReportService> _mockExpenseReportService;
        private readonly Mock<IMailService> _mockMailService;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public ExpenseServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockReadRepository = new Mock<IReadRepository<Expens>>();
            _mockWriteRepository = new Mock<IWriteRepository<Expens>>();
            _mockExpenseRules = new Mock<ExpenseRules>();
            _mockCreditCardService = new Mock<ICreditCardService>();
            _mockExpenseReportService = new Mock<IPdfReportService>();
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


            var config = new MapperConfiguration(cfg =>
            {
               
            });

            _mapper = config.CreateMapper();

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Expens>()).Returns(_mockReadRepository.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Expens>()).Returns(_mockWriteRepository.Object);

            _service = new ExpenseService(_mockUnitOfWork.Object, _mockExpenseRules.Object,_mapper, _mockCreditCardService.Object, _mockExpenseReportService.Object,_mockMailService.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_AddsExpenseAndSaves()
        {
            // Arrange
            var userId = 1;
            var request = new CreateExpenseCommand
            {
                Name = "Yemek",
                Amount = 100
            };

            var existingExpenses = new List<Expens>();

            _mockReadRepository.Setup(r => r.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Expens, bool>>>(), null, null, false))
                .ReturnsAsync(existingExpenses);

            _mockExpenseRules.Setup(r => r.ExpenseNameNotMustBeSame(existingExpenses, request.Name))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(r => r.AddAsync(It.IsAny<Expens>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await _service.CreateAsync(request, userId);

            // Assert
            _mockReadRepository.Verify(r => r.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Expens, bool>>>(), null, null, false), Times.Once);
            _mockExpenseRules.Verify(r => r.ExpenseNameNotMustBeSame(existingExpenses, request.Name), Times.Once);
            _mockWriteRepository.Verify(w => w.AddAsync(It.Is<Expens>(e => e.Name == request.Name && e.Amount == request.Amount && e.UserId == userId)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllExpenseAndPaymentByUserAsync_ShouldReturnCombinedList()
        {
            // Arrange
            var userId = 1;

            var expenses = new List<Expens>
            {
                new Expens { Name = "Market", Amount = 100, UserId = userId }
            };

            var payments = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    Amount = 200,
                    Memberships = new Memberships
                    {
                        User = new User { Id = userId },
                        DigitalPlatform = new DigitalPlatform { Name = "Netflix" }
                    }
                }
            };

            var instructions = new List<Instructions>
                {
                    new Instructions { Id = 1, IsPaid = true, Title = "Otomatik Ödeme", Amount = 150, ScheduledDate = new DateTime(2025, 7, 10), UserId = userId }
                };

            // Mock GetReadRepository<Expens>
            var mockExpenseReadRepo = new Mock<IReadRepository<Expens>>();
            mockExpenseReadRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false))
                .ReturnsAsync(expenses);

            // Mock GetReadRepository<Payment>
            var mockPaymentReadRepo = new Mock<IReadRepository<Payment>>();
            mockPaymentReadRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Payment, bool>>>(),
                It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(),
                null, false))
                .ReturnsAsync(payments);

            // Mock GetReadRepository<Instructions>
            var mockInstructionReadRepo = new Mock<IReadRepository<Instructions>>();
            mockInstructionReadRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(instructions);

            // Setup unit of work
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Expens>()).Returns(mockExpenseReadRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Payment>()).Returns(mockPaymentReadRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Instructions>()).Returns(mockInstructionReadRepo.Object);

            // Mapper mock
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(instructions))
                .Returns(new List<GetAllExpenseAndPaymentByUserQueryResult>
                {
            new() { Name = "Otomatik Ödeme", Amount = 150, PaidDate = new DateTime(2025, 7, 10) }
                });

            mockMapper.Setup(m => m.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(payments))
                .Returns(new List<GetAllExpenseAndPaymentByUserQueryResult>
                {
            new() { Name = "Netflix", Amount = 200, PaidDate = new DateTime(2025, 7, 15) }
                });

            mockMapper.Setup(m => m.Map<IList<GetAllExpenseAndPaymentByUserQueryResult>>(expenses))
                .Returns(new List<GetAllExpenseAndPaymentByUserQueryResult>
                {
            new() { Name = "Market", Amount = 100, PaidDate = default }
                });

            var service = new ExpenseService(_mockUnitOfWork.Object, _mockExpenseRules.Object, _mapper, _mockCreditCardService.Object, _mockExpenseReportService.Object, _mockMailService.Object, _mockUserManager.Object);

            // Act
            var result = await service.GetAllExpenseAndPaymentByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Contains(result, x => x.Name == "Market" && x.Amount == 100);
            Assert.Contains(result, x => x.Name == "Netflix" && x.Amount == 200 && x.PaidDate == new DateTime(2025, 7, 15));
            Assert.Contains(result, x => x.Name == "Otomatik Ödeme" && x.Amount == 150 && x.PaidDate == new DateTime(2025, 7, 10));
        }

        [Fact]
        public async Task GetAllExpensesByUserAsync_ShouldReturnMappedExpenses()
        {
            // Arrange
            var userId = 1;

            var expenses = new List<Expens>
            {
                new Expens { Id = 1, Name = "Kira", Amount = 3000, UserId = userId },
                new Expens { Id = 2, Name = "Elektrik", Amount = 450, UserId = userId }
            };

            var mappedExpenses = new List<GetAllExpenseByUserQueryResult>
            {
                new GetAllExpenseByUserQueryResult { Id = 1, Name = "Kira", Amount = 3000, PaidDate = new DateTime(2025, 7, 1) },
                new GetAllExpenseByUserQueryResult { Id = 2, Name = "Elektrik", Amount = 450, PaidDate = new DateTime(2025, 7, 5) }
            };

            _mockReadRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false))
                .ReturnsAsync(expenses);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<IList<GetAllExpenseByUserQueryResult>>(expenses))
                .Returns(mappedExpenses);

            var service = new ExpenseService(_mockUnitOfWork.Object, _mockExpenseRules.Object, _mapper, _mockCreditCardService.Object, _mockExpenseReportService.Object, _mockMailService.Object, _mockUserManager.Object);

            // Act
            var result = await service.GetAllExpensesByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal("Kira", result[0].Name);
            Assert.Equal(3000, result[0].Amount);
            Assert.Equal(new DateTime(2025, 7, 1), result[0].PaidDate);

            Assert.Equal("Elektrik", result[1].Name);
            Assert.Equal(450, result[1].Amount);
            Assert.Equal(new DateTime(2025, 7, 5), result[1].PaidDate);

            _mockReadRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false), Times.Once);
            mockMapper.Verify(m => m.Map<IList<GetAllExpenseByUserQueryResult>>(expenses), Times.Once);
        }

        [Fact]
        public async Task GetLast3ExpenseByUserAsync_ReturnsTop3ByPaidDateDescending()
        {
            // Arrange
            var userId = 1;

            var expenses = new List<Expens>
            {
                new() { Name = "Market", Amount = 100, UserId = userId }
            };

            var payments = new List<Payment>
            {
                new()
                {
                    Memberships = new Memberships
                    {
                        User = new User { Id = userId },
                        DigitalPlatform = new DigitalPlatform { Name = "Spotify" }
                    },
                    Amount = 200
                }
            };

            var instructions = new List<Instructions>
            {
                new() { Title = "Fatura", Amount = 300, IsPaid = true, ScheduledDate = new DateTime(2025, 7, 19), UserId = userId }
            };

            var mockExpenseRepo = new Mock<IReadRepository<Expens>>();
            var mockPaymentRepo = new Mock<IReadRepository<Payment>>();
            var mockInstructionRepo = new Mock<IReadRepository<Instructions>>();

            mockExpenseRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false)).ReturnsAsync(expenses);
            mockPaymentRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Payment, bool>>>(), It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(), null, false)).ReturnsAsync(payments);
            mockInstructionRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false)).ReturnsAsync(instructions);

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Expens>()).Returns(mockExpenseRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Payment>()).Returns(mockPaymentRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Instructions>()).Returns(mockInstructionRepo.Object);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<IList<GetLast3ExpenseByUserQueryResult>>(instructions)).Returns(new List<GetLast3ExpenseByUserQueryResult>
            {
                new() { Name = "Fatura", Amount = 300, PaidDate = new DateTime(2025, 7, 19) }
            });

            mockMapper.Setup(m => m.Map<IList<GetLast3ExpenseByUserQueryResult>>(payments)).Returns(new List<GetLast3ExpenseByUserQueryResult>
            {
                new() { Name = "Spotify", Amount = 200, PaidDate = new DateTime(2025, 7, 18) }
            });

             mockMapper.Setup(m => m.Map<IList<GetLast3ExpenseByUserQueryResult>>(expenses)).Returns(new List<GetLast3ExpenseByUserQueryResult>
            {
                new() { Name = "Market", Amount = 100, PaidDate = new DateTime(2025, 7, 10) }
            });

            var service = new ExpenseService(_mockUnitOfWork.Object, _mockExpenseRules.Object, _mapper, _mockCreditCardService.Object, _mockExpenseReportService.Object, _mockMailService.Object, _mockUserManager.Object);

            // Act
            var result = await service.GetLast3ExpenseByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Fatura", result[0].Name);      // 2025-07-19
            Assert.Equal("Spotify", result[1].Name);     // 2025-07-18
            Assert.Equal("Market", result[2].Name);      // 2025-07-10
        }

        [Fact]
        public async Task GetLastMonthExpenseTotalAmountAsync_ReturnsCorrectSum()
        {
            // Arrange
            var userId = 1;
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

            var expenses = new List<Expens>
            {
                new() { Amount = 100, CreatedDate = DateTime.UtcNow.AddDays(-10), UserId = userId }
            };

            var payments = new List<Payment>
            {
                new()
                {
                    Amount = 200,
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    Memberships = new Memberships
                    {
                        User = new User { Id = userId },
                        DigitalPlatform = new DigitalPlatform()
                    }
                }
            };

            var instructions = new List<Instructions>
            {
                new()
                {
                    Amount = 300,
                    CreatedDate = DateTime.UtcNow.AddDays(-3),
                    IsPaid = true,
                    UserId = userId
                }
            };

            var mockExpenseRepo = new Mock<IReadRepository<Expens>>();
            var mockPaymentRepo = new Mock<IReadRepository<Payment>>();
            var mockInstructionRepo = new Mock<IReadRepository<Instructions>>();

            mockExpenseRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false))
                .ReturnsAsync(expenses);

            mockPaymentRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Payment, bool>>>(),
                It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(),
                null, false))
                .ReturnsAsync(payments);

            mockInstructionRepo.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false))
                .ReturnsAsync(instructions);

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Expens>()).Returns(mockExpenseRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Payment>()).Returns(mockPaymentRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetReadRepository<Instructions>()).Returns(mockInstructionRepo.Object);

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(expenses))
                .Returns(expenses.Select(e => new GetLastMonthExpenseTotalAmountQueryResult { Amount = e.Amount }).ToList());

            mockMapper.Setup(m => m.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(payments))
                .Returns(payments.Select(p => new GetLastMonthExpenseTotalAmountQueryResult { Amount = p.Amount }).ToList());

            mockMapper.Setup(m => m.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(instructions))
                .Returns(instructions.Select(i => new GetLastMonthExpenseTotalAmountQueryResult { Amount = i.Amount }).ToList());

            var service = new ExpenseService(_mockUnitOfWork.Object, _mockExpenseRules.Object, _mapper, _mockCreditCardService.Object, _mockExpenseReportService.Object, _mockMailService.Object, _mockUserManager.Object);

            // Act
            var result = await service.GetLastMonthExpenseTotalAmountAsync(userId);

            // Assert
            Assert.Equal(600m, result); // 100 + 200 + 300 = 600

            mockExpenseRepo.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, null, false), Times.Once);
            mockPaymentRepo.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Payment, bool>>>(), It.IsAny<Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>>(), null, false), Times.Once);
            mockInstructionRepo.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Instructions, bool>>>(), null, null, false), Times.Once);
            mockMapper.Verify(m => m.Map<IList<GetLastMonthExpenseTotalAmountQueryResult>>(It.IsAny<IEnumerable<object>>()), Times.Exactly(3));
        }

        [Fact]
        public async Task RemoveExpenseAsync_ValidExpense_DeletesAndSaves()
        {
            // Arrange
            int expenseId = 1;
            int userId = 10;

            var expense = new Expens { Id = expenseId, UserId = userId };

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Expens>()).Returns(_mockReadRepository.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<Expens>()).Returns(_mockWriteRepository.Object);

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false))
                .ReturnsAsync(expense);

            _mockExpenseRules.Setup(r => r.ExpensNotFound(expense))
                .Returns(Task.CompletedTask);

            _mockExpenseRules.Setup(r => r.IsThisYourExpense(expense, userId))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(w => w.HardDeleteAsync(expense))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.RemoveExpenseAsync(expenseId, userId);

            // Assert
            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false), Times.Once);
            _mockExpenseRules.Verify(r => r.ExpensNotFound(expense), Times.Once);
            _mockExpenseRules.Verify(r => r.IsThisYourExpense(expense, userId), Times.Once);
            _mockWriteRepository.Verify(w => w.HardDeleteAsync(expense), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoveExpenseAsync_ExpenseNotFound_ThrowsException()
        {
            // Arrange
            int expenseId = 1;
            int userId = 10;

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false))
                .ReturnsAsync((Expens)null);

            _mockExpenseRules.Setup(r => r.ExpensNotFound(null))
                .ThrowsAsync(new Exception("Expense not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.RemoveExpenseAsync(expenseId, userId));

            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false), Times.Once);
            _mockExpenseRules.Verify(r => r.ExpensNotFound(null), Times.Once);
        }

        [Fact]
        public async Task UpdateExpenseAsync_ValidExpense_UpdatesAndSaves()
        {
            // Arrange
            int expenseId = 1;
            int userId = 10;
            string newName = "Updated Expense";
            decimal newAmount = 500m;

            var expense = new Expens { Id = expenseId, UserId = userId, Name = "Old Name", Amount = 100m };

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false))
                .ReturnsAsync(expense);

            _mockExpenseRules.Setup(r => r.ExpensNotFound(expense))
                .Returns(Task.CompletedTask);

            _mockExpenseRules.Setup(r => r.IsThisYourExpense(expense, userId))
                .Returns(Task.CompletedTask);

            _mockWriteRepository.Setup(w => w.UpdateAsync(expense))
                .ReturnsAsync(expense);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ReturnsAsync(1);

            // Act
            await _service.UpdateExpenseAsync(expenseId, newName, newAmount, userId);

            // Assert
            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false), Times.Once);
            _mockExpenseRules.Verify(r => r.ExpensNotFound(expense), Times.Once);
            _mockExpenseRules.Verify(r => r.IsThisYourExpense(expense, userId), Times.Once);
            _mockWriteRepository.Verify(w => w.UpdateAsync(It.Is<Expens>(e => e.Name == newName && e.Amount == newAmount)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);

            Assert.Equal(newName, expense.Name);
            Assert.Equal(newAmount, expense.Amount);
        }

        [Fact]
        public async Task UpdateExpenseAsync_ExpenseNotFound_ThrowsException()
        {
            // Arrange
            int expenseId = 1;
            int userId = 10;
            string newName = "Updated Expense";
            decimal newAmount = 500m;

            _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false))
                .ReturnsAsync((Expens)null);

            _mockExpenseRules.Setup(r => r.ExpensNotFound(null))
                .ThrowsAsync(new Exception("Expense not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateExpenseAsync(expenseId, newName, newAmount, userId));

            _mockReadRepository.Verify(r => r.GetAsync(It.IsAny<Expression<Func<Expens, bool>>>(), null, false), Times.Once);
            _mockExpenseRules.Verify(r => r.ExpensNotFound(null), Times.Once);
        }




    }
}
