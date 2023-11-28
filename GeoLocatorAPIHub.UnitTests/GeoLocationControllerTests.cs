using GeoLocatorApiHub.Models;
using GeoLocatorApiHub.Services.Interfaces;
using GeoLocatorApiHub.Services.Services;
using GeoLocatorAPIHub.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GeoLocatorAPIHub.UnitTests
{
    public class GeoLocationControllerTests
    {
        private readonly Mock<IGeoLocationService> _mockGeoLocationService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly MyExternalService _mockMyExternalService;
        private readonly GeoLocationController _controller;

        public GeoLocationControllerTests()
        {
            _mockGeoLocationService = new Mock<IGeoLocationService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["ExternalApiSettings:ApiKey"]).Returns("mock-api-key");

            _mockMyExternalService = new MyExternalService(_mockConfiguration.Object);
            _controller = new GeoLocationController(_mockGeoLocationService.Object, _mockMyExternalService);
        }

        [Fact]
        public async Task GetLatestGeoLocation_ReturnsNotFound_WhenGeoLocationNotExists()
        {
            var testIp = "127.0.0.1";
            _mockGeoLocationService.Setup(s => s.GetLatestGeoLocationByIpAsync(testIp))
                        .ReturnsAsync((GeoLocationDto)null);

            var result = await _controller.GetLatestGeoLocation(testIp);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllGeoLocations_ReturnsOkResult_WithGeoLocations()
        {
            var testIp = "127.0.0.1";
            var testGeoLocations = new List<GeoLocationDto> { };
            _mockGeoLocationService.Setup(s => s.GetAllGeoLocationsByIpAsync(testIp))
                        .ReturnsAsync(testGeoLocations);

            var result = await _controller.GetAllGeoLocations(testIp);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testGeoLocations, okResult.Value);
        }

        [Fact]
        public async Task AddGeoLocation_ReturnsCreatedAtAction_WhenSuccessful()
        {
            var ipOrUrl = "127.0.0.1";
            var apiKey = string.Empty;
            _mockGeoLocationService.Setup(s => s.AddGeoLocationAsync(ipOrUrl, apiKey))
                        .Returns(Task.CompletedTask);

            var result = await _controller.AddGeoLocation(ipOrUrl);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetLatestGeoLocation), createdAtActionResult.ActionName);
            Assert.Equal(ipOrUrl, createdAtActionResult.RouteValues["ipOrUrl"]);
        }

        [Fact]
        public async Task AddGeoLocation_ReturnsBadRequest_WhenExceptionThrown()
        {
            var ipOrUrl = "127.0.0.1";
            var apiKey = "test-api-key"; // Przykładowy klucz API

            _mockGeoLocationService.Setup(s => s.AddGeoLocationAsync(ipOrUrl, It.IsAny<string>()))
                      .ThrowsAsync(new InvalidOperationException("Test Exception"));

            var result = await _controller.AddGeoLocation(ipOrUrl);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test Exception", badRequestResult.Value);
        }

        [Fact]
        public async Task AddGeoLocation_ReturnsBadRequest_ForInvalidIpAddress()
        {
            var invalidIp = "invalid-ip-address";

            var result = await _controller.AddGeoLocation(invalidIp);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Provided string is not a valid IPv4 or IPv6 address.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteGeoLocation_ReturnsNoContent_WhenSuccessful()
        {
            var ip = "127.0.0.1";
            _mockGeoLocationService.Setup(s => s.DeleteGeoLocationAsync(ip))
                        .ReturnsAsync(true);

            var result = await _controller.DeleteGeoLocation(ip);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteGeoLocation_ReturnsNotFound_WhenGeoLocationNotExists()
        {
            var ip = "127.0.0.1";
            _mockGeoLocationService.Setup(s => s.DeleteGeoLocationAsync(ip))
                        .ReturnsAsync(false);

            var result = await _controller.DeleteGeoLocation(ip);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteGeoLocation_ReturnsBadRequest_WhenExceptionThrown()
        {
            var ip = "127.0.0.1";
            _mockGeoLocationService.Setup(s => s.DeleteGeoLocationAsync(ip))
                        .ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.DeleteGeoLocation(ip);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test Exception", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllGeoLocations_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var ip = "127.0.0.1";
            var exceptionMessage = "Test exception";
            _mockGeoLocationService.Setup(s => s.GetAllGeoLocationsByIpAsync(ip))
                        .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAllGeoLocations(ip);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
    }
}
