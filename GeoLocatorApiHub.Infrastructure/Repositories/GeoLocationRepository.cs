using GeoLocatorApiHub.Infrastructure.Data;
using GeoLocatorApiHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoLocatorApiHub.Infrastructure.Repositories
{
    public class GeoLocationRepository : IGeoLocationRepository
    {
        private readonly GeoLocatorApiHubContext _context;

        public GeoLocationRepository(GeoLocatorApiHubContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<GeoLocation>>> GetGeoLocationsByIpAsync(string ip)
        {
            var geoLocations = await _context.GeoLocations
                .Where(g => g.Ip == ip)
                .ToListAsync();

            if (geoLocations != null && geoLocations.Any())
            {
                return OperationResult<IEnumerable<GeoLocation>>.Success(geoLocations);
            }
            return OperationResult<IEnumerable<GeoLocation>>.Failure("No GeoLocation entries found for the provided IP.");
        }

        public async Task<OperationResult<GeoLocation>> GetLatestGeoLocationByIpAsync(string ip)
        {
            var latestGeoLocation = await _context.GeoLocations
                .Where(g => g.Ip == ip)
                .OrderByDescending(g => g.CreatedDate)
                .FirstOrDefaultAsync();

            if (latestGeoLocation != null)
            {
                return OperationResult<GeoLocation>.Success(latestGeoLocation);
            }
            return OperationResult<GeoLocation>.Failure("No latest GeoLocation entry found for the provided IP.");
        }

        public async Task<OperationResult<GeoLocation>> AddGeoLocationAsync(GeoLocation geoLocation)
        {
            try
            {
                geoLocation.CreatedDate = DateTime.UtcNow;
                await _context.GeoLocations.AddAsync(geoLocation);
                await _context.SaveChangesAsync();
                return OperationResult<GeoLocation>.Success(geoLocation);
            }
            catch (Exception ex)
            {
                return OperationResult<GeoLocation>.Failure($"Failed to add GeoLocation: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteGeoLocationAsync(string ip)
        {
            var geoLocation = await _context.GeoLocations.FirstOrDefaultAsync(g => g.Ip == ip);
            if (geoLocation == null)
            {
                return OperationResult<bool>.Failure("GeoLocation not found for the provided IP.");
            }

            try
            {
                _context.GeoLocations.Remove(geoLocation);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to delete GeoLocation: {ex.Message}");
            }
        }
    }
}
