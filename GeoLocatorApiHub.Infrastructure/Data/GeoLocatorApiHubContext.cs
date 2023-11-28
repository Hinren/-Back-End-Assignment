using GeoLocatorApiHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoLocatorApiHub.Infrastructure.Data
{
    public class GeoLocatorApiHubContext : DbContext
    {
        public GeoLocatorApiHubContext(DbContextOptions<GeoLocatorApiHubContext> options)
        : base(options)
        {
        }

        public DbSet<GeoLocation> GeoLocations { get; set; }
    }
}
