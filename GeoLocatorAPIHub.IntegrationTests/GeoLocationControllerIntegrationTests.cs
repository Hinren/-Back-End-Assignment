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
        //    // Przyk�adowe IP dla testu
        //    var testIp = "127.0.0.1";

        //    // Wykonaj ��danie do API
        //    var response = await _client.GetAsync($"/api/geolocations/{testIp}");

        //    // Sprawd�, czy odpowied� jest poprawna
        //    response.EnsureSuccessStatusCode();

        //    // Mo�esz tutaj doda� dodatkowe weryfikacje, np. sprawdzi� tre�� odpowiedzi
        //}
    }
}