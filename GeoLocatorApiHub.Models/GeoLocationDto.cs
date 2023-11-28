namespace GeoLocatorApiHub.Models
{
    public class GeoLocationDto
    {
        public int Id { get; set; }

        public required string Ip { get; set; }

        public required string Type { get; set; }

        public required string ContinentCode { get; set; }

        public required string ContinentName { get; set; }

        public required string CountryCode { get; set; }

        public required string CountryName { get; set; }

        public required string RegionCode { get; set; }

        public required string RegionName { get; set; }

        public required string City { get; set; }

        public required string Zip { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}