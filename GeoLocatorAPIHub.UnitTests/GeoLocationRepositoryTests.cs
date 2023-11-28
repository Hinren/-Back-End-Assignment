using AutoFixture;
using GeoLocatorApiHub.Infrastructure.Data;
using GeoLocatorApiHub.Infrastructure.Entities;
using GeoLocatorApiHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeoLocatorAPIHub.UnitTests
{
    public class GeoLocationRepositoryTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly DbContextOptions<GeoLocatorApiHubContext> _dbContextOptions;

        public GeoLocationRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<GeoLocatorApiHubContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new GeoLocatorApiHubContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetGeoLocationsByIpAsync_ReturnsGeoLocations_WhenExists()
        {
            // Arrange
            using var context = new GeoLocatorApiHubContext(_dbContextOptions);
            var repo = new GeoLocationRepository(context);

            var testIp = "127.0.0.1";
            var fakeGeoLocations = _fixture.Build<GeoLocation>()
                                           .With(g => g.Ip, testIp)
                                           .CreateMany(5).ToList();

            context.GeoLocations.AddRange(fakeGeoLocations);
            context.SaveChanges();

            // Act
            var result = await repo.GetGeoLocationsByIpAsync(testIp);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(5, result.Data.Count());
            foreach (var geoLocation in result.Data)
            {
                Assert.Equal(testIp, geoLocation.Ip);
            }
        }

        [Fact]
        public async Task GetLatestGeoLocationByIpAsync_ReturnsLatestGeoLocation_WhenExists()
        {
            using var context = new GeoLocatorApiHubContext(_dbContextOptions);
            var repo = new GeoLocationRepository(context);

            var testIp = "127.0.0.1";
            var fakeGeoLocations = _fixture.Build<GeoLocation>()
                                           .With(g => g.Ip, testIp)
                                           .CreateMany(3).ToList();

            fakeGeoLocations[0].CreatedDate = DateTime.Now.AddDays(-3);
            fakeGeoLocations[1].CreatedDate = DateTime.Now.AddDays(-1);
            fakeGeoLocations[2].CreatedDate = DateTime.Now.AddDays(-2);

            context.GeoLocations.AddRange(fakeGeoLocations);
            context.SaveChanges();

            // Act
            var result = await repo.GetLatestGeoLocationByIpAsync(testIp);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(fakeGeoLocations[1].CreatedDate, result.Data.CreatedDate);
        }

        [Fact]
        public async Task AddGeoLocationAsync_AddsGeoLocationSuccessfully()
        {
            using var context = new GeoLocatorApiHubContext(_dbContextOptions);
            var repo = new GeoLocationRepository(context);

            var fakeGeoLocation = _fixture.Create<GeoLocation>();

            // Act
            var result = await repo.AddGeoLocationAsync(fakeGeoLocation);

            // Assert
            Assert.True(result.IsSuccess);
            var addedItem = context.GeoLocations.FirstOrDefault(g => g.Ip == fakeGeoLocation.Ip);
            Assert.NotNull(addedItem);
            Assert.Equal(fakeGeoLocation.City, addedItem.City);
        }

        [Fact]
        public async Task DeleteGeoLocationAsync_DeletesGeoLocation_WhenExists()
        {
            using var context = new GeoLocatorApiHubContext(_dbContextOptions);
            var repo = new GeoLocationRepository(context);

            var testIp = "127.0.0.1";
            var fakeGeoLocation = _fixture.Build<GeoLocation>()
                                          .With(g => g.Ip, testIp)
                                          .Create();

            context.GeoLocations.Add(fakeGeoLocation);
            context.SaveChanges();

            // Act
            var result = await repo.DeleteGeoLocationAsync(testIp);

            // Assert
            Assert.True(result.IsSuccess);
            var deletedItem = context.GeoLocations.FirstOrDefault(g => g.Ip == testIp);
            Assert.Null(deletedItem);
        }
    }
}
