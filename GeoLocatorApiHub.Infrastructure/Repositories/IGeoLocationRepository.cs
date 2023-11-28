using GeoLocatorApiHub.Infrastructure.Entities;

namespace GeoLocatorApiHub.Infrastructure.Repositories
{
    public interface IGeoLocationRepository
    {
        Task<OperationResult<IEnumerable<GeoLocation>>> GetGeoLocationsByIpAsync(string ip);
        Task<OperationResult<GeoLocation>> GetLatestGeoLocationByIpAsync(string ip);
        Task<OperationResult<GeoLocation>> AddGeoLocationAsync(GeoLocation geoLocation);
        Task<OperationResult<bool>> DeleteGeoLocationAsync(string ip);
    }
}
