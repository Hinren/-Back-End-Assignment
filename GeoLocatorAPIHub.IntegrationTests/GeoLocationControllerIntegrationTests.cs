using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GeoLocatorAPIHub.IntegrationTests
{
    public class GeoLocationControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        //private readonly WebApplicationFactory<Program> _factory;
        //private readonly HttpClient _client;

        //public GeoLocationControllerIntegrationTests(WebApplicationFactory<Program> factory)
        //{
        //    _factory = factory;
        //    _client = factory.CreateClient();
        //}

        //[Fact]
        //public async Task GetAllGeoLocations_ReturnsOkResponse()
        //{
        //    var testIp = "127.0.0.1";

        //    var response = await _client.GetAsync($"/api/geolocations/{testIp}");

        //    response.EnsureSuccessStatusCode();

        //}
    }
}