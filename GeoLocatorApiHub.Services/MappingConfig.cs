using GeoLocatorApiHub.Infrastructure.Entities;
using GeoLocatorApiHub.Models;
using Mapster;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<GeoLocation, GeoLocationDto>.NewConfig();
        TypeAdapterConfig<ApiIpstackResponseJson, GeoLocation>.NewConfig()
            .Map(dest => dest.Ip, src => src.ip)
            .Map(dest => dest.ContinentName, src => src.continent_name)
            .Map(dest => dest.ContinentCode, src => src.continent_code)
            .Map(dest => dest.CountryCode, src => src.country_code)
            .Map(dest => dest.CountryName, src => src.country_name)
            .Map(dest => dest.City, src => src.city)
            .Map(dest => dest.Zip, src => src.zip);
    }
}
