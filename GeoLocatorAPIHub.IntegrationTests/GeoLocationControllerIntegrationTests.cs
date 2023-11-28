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
        //    // Przyk³adowe IP dla testu
        //    var testIp = "127.0.0.1";

        //    // Wykonaj ¿¹danie do API
        //    var response = await _client.GetAsync($"/api/geolocations/{testIp}");

        //    // SprawdŸ, czy odpowiedŸ jest poprawna
        //    response.EnsureSuccessStatusCode();

        //    // Mo¿esz tutaj dodaæ dodatkowe weryfikacje, np. sprawdziæ treœæ odpowiedzi
        //}
    }
}