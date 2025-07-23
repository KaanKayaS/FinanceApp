using AutoMapper;
using FinanceApp.Application.Features.Results.DigitalPlatformResults;
using FinanceApp.Application.Interfaces.Repositories;
using FinanceApp.Application.Interfaces.UnitOfWorks;
using FinanceApp.Domain.Entities;
using FinanceApp.Persistence.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests
{
   
        public class DigitalPlatformServiceTests
        {
            private readonly Mock<IUnitOfWork> _mockUnitOfWork;
            private readonly Mock<IReadRepository<DigitalPlatform>> _mockReadRepository;
            private readonly Mock<IWriteRepository<DigitalPlatform>> _mockWriteRepository;
            private readonly IMapper _mapper;
            private readonly DigitalPlatformService _service;

            public DigitalPlatformServiceTests()
            {
                _mockUnitOfWork = new Mock<IUnitOfWork>();
                _mockReadRepository = new Mock<IReadRepository<DigitalPlatform>>();
                _mockWriteRepository = new Mock<IWriteRepository<DigitalPlatform>>();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DigitalPlatform, GetAllDigitalPlatfomQueryResult>();
                });

                _mapper = config.CreateMapper();

                _mockUnitOfWork.Setup(u => u.GetReadRepository<DigitalPlatform>()).Returns(_mockReadRepository.Object);
                _mockUnitOfWork.Setup(u => u.GetWriteRepository<DigitalPlatform>()).Returns(_mockWriteRepository.Object);

                _service = new DigitalPlatformService(_mockUnitOfWork.Object, _mapper);
            }

            [Fact]
            public async Task GetAllAsync_ShouldReturnMappedDigitalPlatforms()
                {
                    // Arrange
                    var platforms = new List<DigitalPlatform>
                    {
                        new DigitalPlatform
                        {
                            Id = 1,
                            Name = "Netflix",
                            ImagePath = "netflix.png",
                            CreatedDate = new DateTime(2023, 1, 1)
                        },
                        new DigitalPlatform
                        {
                            Id = 2,
                            Name = "Spotify",
                            ImagePath = "spotify.png",
                            CreatedDate = new DateTime(2024, 1, 1)
                        }
                    };

                    _mockReadRepository
                        .Setup(repo => repo.GetAllAsync(null, null, null, false))
                        .ReturnsAsync(platforms);

                    // Act
                    var result = await _service.GetAllAsync();

                    // Assert
                    result.Should().NotBeNull();
                    result.Should().HaveCount(2);

                    result[0].Id.Should().Be(1);
                    result[0].Name.Should().Be("Netflix");
                    result[0].ImagePath.Should().Be("netflix.png");
                    result[0].CreatedDate.Should().Be(new DateTime(2023, 1, 1));

                    result[1].Id.Should().Be(2);
                    result[1].Name.Should().Be("Spotify");
                    result[1].ImagePath.Should().Be("spotify.png");
                    result[1].CreatedDate.Should().Be(new DateTime(2024, 1, 1));
                }

            [Fact]
            public async Task UploadPlatformImageAsync_ValidFile_UpdatesImagePathAndSaves()
            {
                // Arrange
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "platforms");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var platformId = 1;
                    var platform = new DigitalPlatform { Id = platformId, Name = "Netflix", ImagePath = null };

                    _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DigitalPlatform, bool>>>(), null, false))
                        .ReturnsAsync(platform);

                    _mockWriteRepository.Setup(w => w.UpdateAsync(platform))
                        .ReturnsAsync(platform);

                    _mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(1);

                    // Mock IFormFile
                    var content = "Fake file content";
                    var fileName = "test.png";
                    var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
                    var mockFormFile = new Mock<IFormFile>();
                    mockFormFile.Setup(f => f.FileName).Returns(fileName);
                    mockFormFile.Setup(f => f.Length).Returns(ms.Length);
                    mockFormFile.Setup(f => f.OpenReadStream()).Returns(ms);
                    mockFormFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                                .Returns<Stream, CancellationToken>((stream, token) =>
                                {
                                    ms.Position = 0;
                                    return ms.CopyToAsync(stream, 81920, token);
                                });

                    // Act
                    await _service.UploadPlatformImageAsync(platformId, mockFormFile.Object, CancellationToken.None);

                    // Assert
                    platform.ImagePath.Should().StartWith("images/platforms/");
                    _mockWriteRepository.Verify(w => w.UpdateAsync(platform), Times.Once);
                    _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            }

            [Fact]
            public async Task UploadPlatformImageAsync_PlatformNotFound_ThrowsException()
            {
                // Arrange
                var platformId = 999;
                _mockReadRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DigitalPlatform, bool>>>(), null, false))
                    .ReturnsAsync((DigitalPlatform)null);

                var mockFormFile = new Mock<IFormFile>();

                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() =>
                    _service.UploadPlatformImageAsync(platformId, mockFormFile.Object, CancellationToken.None));
            }
    }
 }

