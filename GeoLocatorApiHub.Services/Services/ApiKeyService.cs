using GeoLocatorApiHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GeoLocatorApiHub.Services.Services
{
    public class ApiKeyService : IApiKeyService
    {
        public readonly string? apiKey;

        public ApiKeyService(IConfiguration configuration)
        {
            apiKey = configuration["ExternalApiSettings:ApiKey"];
        }
    }
}
