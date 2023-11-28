using GeoLocatorApiHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GeoLocatorApiHub.Services.Services
{
    public class MyExternalService : IMyExternalService
    {
        public readonly string? apiKey;

        public MyExternalService(IConfiguration configuration)
        {
            apiKey = configuration["ExternalApiSettings:ApiKey"];
        }
    }
}
