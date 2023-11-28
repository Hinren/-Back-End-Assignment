using GeoLocatorApiHub.Models;

namespace GeoLocatorApiHub.Services.Interfaces
{
    public interface IGeoLocationService
    {
        Task<IEnumerable<GeoLocationDto>> GetAllGeoLocationsByIpAsync(string ip);
        Task<GeoLocationDto> GetLatestGeoLocationByIpAsync(string ip);
        Task AddGeoLocationAsync(string ip, string? apiKey);
        Task<bool> DeleteGeoLocationAsync(string ip);
    }
}
