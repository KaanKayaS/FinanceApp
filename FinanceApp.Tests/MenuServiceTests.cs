using AutoMapper;
using FinanceApp.Application.Features.Results.MenuResults;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
    public class MenuServiceTests
    {
        private readonly Mock<IReadRepository<Menu>> _mockMenuReadRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MenuService _service;

        public MenuServiceTests()
        {
            _mockMenuReadRepo = new Mock<IReadRepository<Menu>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _mockUnitOfWork.Setup(u => u.GetReadRepository<Menu>())
                           .Returns(_mockMenuReadRepo.Object);

            _service = new MenuService(_mockMapper.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllMenusAsync_ShouldReturnMappedMenuResults()
        {
            // Arrange
            var menus = new List<Menu>
            {
                new Menu { Id = 1, Name = "Ana Menü", ParentId = null, Order = 1 },
                new Menu { Id = 2, Name = "Alt Menü", ParentId = 1, Order = 2 }
            };

            var expectedResults = new List<GetAllMenuBarQueryResult>
            {
                new GetAllMenuBarQueryResult { Id = 1, Name = "Ana Menü", ParentId = null, Order = 1 },
                new GetAllMenuBarQueryResult { Id = 2, Name = "Alt Menü", ParentId = 1, Order = 2 }
            };

            _mockMenuReadRepo.Setup(repo => repo.GetAllAsync(null, null, null, false))
                             .ReturnsAsync(menus);

            _mockMapper.Setup(m => m.Map<IList<GetAllMenuBarQueryResult>>(menus))
                       .Returns(expectedResults);

            // Act
            var result = await _service.GetAllMenusAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Ana Menü", result[0].Name);
            Assert.Equal(1, result[1].ParentId);

            _mockMenuReadRepo.Verify(repo => repo.GetAllAsync(null, null, null, false), Times.Once);
            _mockMapper.Verify(m => m.Map<IList<GetAllMenuBarQueryResult>>(menus), Times.Once);
        }

    }
}
