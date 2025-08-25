using AutoMapper;
using FinanceApp.Application.Features.Commands.CreditCardCommands;
using FinanceApp.Application.Features.Results.CreditCardResults;
using FinanceApp.Application.Features.Rules;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.Services;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class CreditCardServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<CreditCardRules> _mockCreditCardRules;
        private readonly Mock<AuthRules> _mockAuthRules;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreditCardService _service;
        private readonly IMapper _mapper;


        private readonly Mock<IReadRepository<CreditCard>> _mockReadRepo;
        private readonly Mock<IWriteRepository<CreditCard>> _mockWriteCreditCardRepo;
        private readonly Mock<IWriteRepository<BalanceMemory>> _mockWriteBalanceMemoryRepo;

        public CreditCardServiceTests()
        {
            // Mapper konfigürasyonu
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreditCard, GetAllCreditCardsByUserQueryResult>()
                    .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.Id));
            });

            _mapper = config.CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCreditCardRules = new Mock<CreditCardRules>();
            _mockAuthRules = new Mock<AuthRules>();             

            _mockReadRepo = new Mock<IReadRepository<CreditCard>>();
            _mockWriteCreditCardRepo = new Mock<IWriteRepository<CreditCard>>();
            _mockWriteBalanceMemoryRepo = new Mock<IWriteRepository<BalanceMemory>>();

            _mockUnitOfWork.Setup(u => u.GetReadRepository<CreditCard>()).Returns(_mockReadRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<CreditCard>()).Returns(_mockWriteCreditCardRepo.Object);
            _mockUnitOfWork.Setup(u => u.GetWriteRepository<BalanceMemory>()).Returns(_mockWriteBalanceMemoryRepo.Object);

            _service = new CreditCardService(
                _mockUnitOfWork.Object,
                _mockCreditCardRules.Object,
                _mockAuthRules.Object,
                _mapper
                );
        }

        [Fact]
        public async Task AddBalanceAsync_ValidRequest_ShouldUpdateBalanceAndAddBalanceMemory()
        {
            // Arrange
            var creditCard = new CreditCard { Id = 1, UserId = 123, Balance = 100 };
            var request = new AddBalanceCreditCardCommand { Id = 1, Balance = 50 };

            _mockReadRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<CreditCard, bool>>>() ,null,false))
                         .ReturnsAsync(creditCard);

            // Act
            await _service.AddBalanceAsync(request, 123);

            // Assert
            creditCard.Balance.Should().Be(150);
            _mockCreditCardRules.Verify(r => r.CreditCardNoNotFound(creditCard), Times.Once);
            _mockCreditCardRules.Verify(r => r.DoesThisCardBelongToYou(creditCard, 123), Times.Once);
            _mockWriteCreditCardRepo.Verify(w => w.UpdateAsync(creditCard), Times.Once);
            _mockWriteBalanceMemoryRepo.Verify(w => w.AddAsync(It.Is<BalanceMemory>(b =>
                b.Name == "Para Yükleme" && b.Amount == 50 && b.CreditCardId == 1
            )), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCreditCardSuccessfully()
        {
            // Arrange
            var request = new CreateCreditCardCommand
            {
                CardNo = "1234567890123456",
                ValidDate = "09/28",
                Cvv = "123",
                NameOnCard = "Kaan Kaya"
            };

            var userId = 1;
            var existingCards = new List<CreditCard>();

            _mockReadRepo.Setup(r => r.GetAllAsync(null,null,null, false))
                         .ReturnsAsync(existingCards);

            _mockCreditCardRules.Setup(r => r.CreditCardNoNotBeSame(existingCards, request.CardNo))
                                .Returns(Task.CompletedTask);

            CreditCard? addedCard = null;
            _mockWriteCreditCardRepo.Setup(r => r.AddAsync(It.IsAny<CreditCard>()))
                          .Callback<CreditCard>(card => addedCard = card)
                          .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

            // Act
            await _service.CreateAsync(request, userId);

            // Assert
            _mockReadRepo.Verify(r => r.GetAllAsync(null,null,null, false), Times.Once);
            _mockCreditCardRules.Verify(r => r.CreditCardNoNotBeSame(existingCards, request.CardNo), Times.Once);
            _mockWriteCreditCardRepo.Verify(r => r.AddAsync(It.IsAny<CreditCard>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);

            addedCard.Should().NotBeNull();
            addedCard.CardNo.Should().Be("1234567890123456");
            addedCard.UserId.Should().Be(1);
            addedCard.ValidDate.Should().Be(request.ValidDate);
            addedCard.Cvv.Should().Be("123");
            addedCard.NameOnCard.Should().Be("Kaan Kaya");
            addedCard.Balance.Should().Be(0);
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnMappedCreditCards()
        {
            // Arrange
            int userId = 1;
            var creditCards = new List<CreditCard>
            {
                new CreditCard
                {
                    Id = 1,
                    CardNo = "1234567890123456",
                    ValidDate = "12/25",
                    Cvv = "123",
                    NameOnCard = "Kaan Kaya",
                    Balance = 1000,
                    UserId = userId
                },
                new CreditCard
                {
                    Id = 2,
                    CardNo = "9876543210987654",
                    ValidDate = "11/24",
                    Cvv = "456",
                    NameOnCard = "Beyza Nur",
                    Balance = 2000,
                    UserId = userId
                }
            };

            _mockReadRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<Expression<Func<CreditCard, bool>>>(), null,null, false))
                .ReturnsAsync(creditCards);

            // Act
            var result = await _service.GetAllByUserAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result[0].CardId.Should().Be(1);
            result[0].CardNo.Should().Be("1234567890123456");
            result[0].ValidDate.Should().Be("12/25");
            result[0].Cvv.Should().Be("123");
            result[0].NameOnCard.Should().Be("Kaan Kaya");
            result[0].Balance.Should().Be(1000);

            result[1].CardId.Should().Be(2);
            result[1].CardNo.Should().Be("9876543210987654");
            result[1].ValidDate.Should().Be("11/24");
            result[1].Cvv.Should().Be("456");
            result[1].NameOnCard.Should().Be("Beyza Nur");
            result[1].Balance.Should().Be(2000);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteCreditCard_WhenValid()
        {
            // Arrange
            int creditCardId = 1;
            int userId = 123;

            var creditCard = new CreditCard
            {
                Id = creditCardId,
                UserId = userId,
                CardNo = "1234567890123456",
                ValidDate = "12/25",
                Cvv = "123",
                NameOnCard = "Kaan Kaya",
                Balance = 1000
            };

            _mockReadRepo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<CreditCard, bool>>>(), null, false))
                         .ReturnsAsync(creditCard);

            _mockCreditCardRules.Setup(r => r.CreditCardNoNotFound(creditCard))
                                .Returns(Task.CompletedTask);

            _mockCreditCardRules.Setup(r => r.DoesThisCardBelongToYou(creditCard, userId))
                                .Returns(Task.CompletedTask);

            _mockWriteCreditCardRepo.Setup(r => r.HardDeleteAsync(creditCard))
                                    .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                           .ReturnsAsync(1);

            // Act
            await _service.RemoveAsync(creditCardId, userId);

            // Assert
            _mockReadRepo.Verify(r => r.GetAsync(It.IsAny<Expression<Func<CreditCard, bool>>>(), null, false), Times.Once);
            _mockCreditCardRules.Verify(r => r.CreditCardNoNotFound(creditCard), Times.Once);
            _mockCreditCardRules.Verify(r => r.DoesThisCardBelongToYou(creditCard, userId), Times.Once);
            _mockWriteCreditCardRepo.Verify(w => w.HardDeleteAsync(creditCard), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

    }
}
