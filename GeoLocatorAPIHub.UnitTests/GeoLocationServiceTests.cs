using GeoLocatorApiHub.Infrastructure.Entities;
using GeoLocatorApiHub.Infrastructure;
using GeoLocatorApiHub.Infrastructure.Repositories;
using GeoLocatorApiHub.Models;
using GeoLocatorApiHub.Services.Services;
using Mapster;
using Moq;
using AutoFixture;
using Moq.Protected;
using System.Net.Http;
using System.Net;
using System.Text.Json;

namespace GeoLocatorAPIHub.UnitTests
{
    public class GeoLocationServiceTests
    {
        private readonly Mock<IGeoLocationRepository> _mockRepo;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly GeoLocationService _service;
        private readonly Fixture _fixture;

        public GeoLocationServiceTests()
        {
            _mockRepo = new Mock<IGeoLocationRepository>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _service = new GeoLocationService(_mockRepo.Object, _mockHttpClientFactory.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllGeoLocationsByIpAsync_ReturnsData_WhenFound()
        {
            // Arrange
            var ip = "127.0.0.1";
            var fakeGeoLocations = _fixture.CreateMany<GeoLocation>(5).ToList();

            _mockRepo.Setup(r => r.GetGeoLocationsByIpAsync(ip))
                .ReturnsAsync(OperationResult<IEnumerable<GeoLocation>>.Success(fakeGeoLocations));

            // Act
            var result = await _service.GetAllGeoLocationsByIpAsync(ip);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeGeoLocations.Count, result.Count());
        }

        [Fact]
        public async Task GetAllGeoLocationsByIpAsync_ReturnsEmpty_WhenNotFound()
        {
            // Arrange
            var ip = "127.0.0.1";
            _mockRepo.Setup(r => r.GetGeoLocationsByIpAsync(ip))
                .ReturnsAsync(OperationResult<IEnumerable<GeoLocation>>.Failure("Not found"));

            // Act
            var result = await _service.GetAllGeoLocationsByIpAsync(ip);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLatestGeoLocationByIpAsync_ReturnsLatestGeoLocation_WhenExists()
        {
            // Arrange
            var ip = "127.0.0.1";
            var fakeGeoLocation = _fixture.Create<GeoLocation>();
            var expectedDto = fakeGeoLocation.Adapt<GeoLocationDto>();
            _mockRepo.Setup(r => r.GetLatestGeoLocationByIpAsync(ip))
                .ReturnsAsync(OperationResult<GeoLocation>.Success(fakeGeoLocation));

            // Act
            var result = await _service.GetLatestGeoLocationByIpAsync(ip);

            // Assert
            Assert.NotNull(result);

            foreach (var property in typeof(GeoLocationDto).GetProperties())
            {
                var expectedValue = property.GetValue(expectedDto);
                var actualValue = property.GetValue(result);
                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public async Task GetLatestGeoLocationByIpAsync_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            var ip = "127.0.0.1";
            _mockRepo.Setup(r => r.GetLatestGeoLocationByIpAsync(ip))
                .ReturnsAsync(OperationResult<GeoLocation>.Failure("No latest GeoLocation entry found for the provided IP."));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetLatestGeoLocationByIpAsync(ip));
            Assert.Equal("GeoLocation not found for the provided IP.", ex.Message);
        }

        [Fact]
        public async Task AddGeoLocationAsync_AddsGeoLocation_WhenSuccessful()
        {
            // Arrange
            var ipOrUrl = "127.0.0.1";
            var apiKey = string.Empty;
            var fakeGeoLocation = _fixture.Create<GeoLocation>();
            var fakeResponseContent = new StringContent(JsonSerializer.Serialize(fakeGeoLocation));
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = fakeResponseContent };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(ipOrUrl)), // Sprawdza, czy URL zawiera IP
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(fakeResponse);

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _mockRepo.Setup(r => r.AddGeoLocationAsync(It.IsAny<GeoLocation>()))
                .ReturnsAsync(OperationResult<GeoLocation>.Success(fakeGeoLocation));

            // Act
            await _service.AddGeoLocationAsync(ipOrUrl, apiKey);

            // Assert
            _mockRepo.Verify(r => r.AddGeoLocationAsync(It.IsAny<GeoLocation>()), Times.Once);
        }

        [Fact]
        public async Task AddGeoLocationAsync_ThrowsException_WhenFailed()
        {
            // Arrange
            var ipOrUrl = "127.0.0.1";
            var apiKey = string.Empty;
            var fakeGeoLocation = _fixture.Create<GeoLocation>();

            // Symulacja nieudanej odpowiedzi HTTP
            var fakeResponse = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Bad Request") };
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(ipOrUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(fakeResponse);

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _mockRepo.Setup(r => r.AddGeoLocationAsync(It.IsAny<GeoLocation>()))
                .ReturnsAsync(OperationResult<GeoLocation>.Failure("Failed to add GeoLocation"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.AddGeoLocationAsync(ipOrUrl, apiKey));
            Assert.Equal("Error retrieving data from ipstack.", ex.Message);
        }

        [Fact]
        public async Task DeleteGeoLocationAsync_DeletesGeoLocation_WhenExists()
        {
            // Arrange
            var ip = "127.0.0.1";
            _mockRepo.Setup(r => r.DeleteGeoLocationAsync(ip))
                .ReturnsAsync(OperationResult<bool>.Success(true));

            // Act
            var result = await _service.DeleteGeoLocationAsync(ip);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteGeoLocationAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var ip = "127.0.0.1";
            _mockRepo.Setup(r => r.DeleteGeoLocationAsync(ip))
                .ReturnsAsync(OperationResult<bool>.Failure("GeoLocation not found for the provided IP."));

            // Act
            var result = await _service.DeleteGeoLocationAsync(ip);

            // Assert
            Assert.False(result);
        }
    }
}