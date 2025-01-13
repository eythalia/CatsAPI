using Moq;
using CatImageApi.API.Controllers;
using CatImageApi.Application.DTOs;
using CatImageApi.Application.Interfaces;
using CatImageApi.Application.Result;
using Microsoft.AspNetCore.Mvc;
using CatImageApi.API.DTOs;
using FluentAssertions;

namespace CatImageApi.Tests
{
    public class CatControllerTests
    {
        private readonly CatController _controller;
        private readonly Mock<ICatService> _mockCatService;

        public CatControllerTests()
        {
            _mockCatService = new Mock<ICatService>();
            _controller = new CatController(_mockCatService.Object);
        }

        [Fact]
        public async Task GetCat_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var invalidId = -1;

            // Act
            var result = await _controller.GetCat(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            badRequestResult.StatusCode.Should().Be(400);

            var errorValue = badRequestResult.Value?.GetType().GetProperty("error")?.GetValue(badRequestResult.Value)?.ToString();
            Assert.Equal("The id must be greater than 0.", errorValue);
        }

        [Fact]
        public async Task GetCat_CatNotFound_ReturnsNotFound()
        {
            // Arrange
            var catId = 1;
            _mockCatService.Setup(s => s.GetCatByIdAsync(catId))
                .ReturnsAsync(Result<CatResponseDTO?>.Failure("Cat not found.", 404));

            // Act
            var result = await _controller.GetCat(catId);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            var errorValue = notFoundResult.Value?.GetType().GetProperty("error")?.GetValue(notFoundResult.Value)?.ToString();
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Cat not found.", errorValue);
        }

        [Fact]
        public async Task GetCat_ValidId_ReturnsOk()
        {
            // Arrange
            var validId = 1;
            var expectedCat = new CatResponseDTO { Id = validId };
            _mockCatService.Setup(s => s.GetCatByIdAsync(validId))
                .ReturnsAsync(Result<CatResponseDTO?>.Success(expectedCat, 200));

            // Act
            var result = await _controller.GetCat(validId);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var dataValue = okResult.Value?.GetType().GetProperty("data")?.GetValue(okResult.Value) as CatResponseDTO;
            Assert.NotNull(dataValue);
            Assert.Equal(expectedCat.Id, dataValue.Id);
           
        }

        [Fact]
        public async Task FetchCats_Success_ReturnsCreated()
        {
            // Arrange
            var responseDto = new CatAdditionResponseDTO
            {
                Succeed = new List<string> { "Cat1", "Cat2" },
                TotalSucceed = 2,
                TotalExisting = 1,
                TotalFailed = 0
            };

            _mockCatService.Setup(s => s.FetchAndStoreCatsAsync())
                .ReturnsAsync(Result<CatAdditionResponseDTO>.Success(responseDto, 201));

            // Act
            var result = await _controller.FetchCats();

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);

            var dataValue = createdResult.Value?.GetType().GetProperty("data")?.GetValue(createdResult.Value) as CatAdditionResponseDTO;
            Assert.NotNull(dataValue);
            Assert.Equal(2, dataValue.TotalSucceed);
            Assert.Equal(1, dataValue.TotalExisting);
            Assert.Equal(0, dataValue.TotalFailed);
        }

        [Fact]
        public async Task FetchCats_Failure_ReturnsError()
        {
            // Arrange
            _mockCatService.Setup(s => s.FetchAndStoreCatsAsync())
                .ReturnsAsync(Result<CatAdditionResponseDTO>.Failure("Failed to fetch cats.", 500));

            // Act
            var result = await _controller.FetchCats();

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);

            var errorValue = errorResult.Value?.GetType().GetProperty("error")?.GetValue(errorResult.Value)?.ToString();
            Assert.Equal("Failed to fetch cats.", errorValue);
        }

        [Fact]
        public async Task GetCats_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new GetCatsRequestDto { page = 1, pagesize = 10, tag = null };
            var responseDto = new PaginatedCatResponseDTO<CatResponseDTO>
            {
                Items = new List<CatResponseDTO>
                {
                    new CatResponseDTO { Id = 1 },
                    new CatResponseDTO { Id = 2 }
                },
                CurrentPage = 1,
                PageSize = 10,
                TotalItems = 2,
                TotalPages = 1
            };

            _mockCatService.Setup(s => s.GetCatsAsync(request.page, request.pagesize, request.tag))
                .ReturnsAsync(Result<PaginatedCatResponseDTO<CatResponseDTO>>.Success(responseDto, 200));

            // Act
            var result = await _controller.GetCats(request);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var dataValue = okResult.Value?.GetType().GetProperty("data")?.GetValue(okResult.Value) as PaginatedCatResponseDTO<CatResponseDTO>;
            Assert.NotNull(dataValue);
            Assert.Equal(2, dataValue.TotalItems);
            Assert.Equal(1, dataValue.CurrentPage);
        }

        [Fact]
        public async Task GetCats_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("page", "Page number is required.");

            // Act
            var result = await _controller.GetCats(new GetCatsRequestDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            
            var errors = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Contains("page", errors.Keys);
            Assert.Contains("Page number is required.", errors["page"] as string[]);
        }
    }

}
