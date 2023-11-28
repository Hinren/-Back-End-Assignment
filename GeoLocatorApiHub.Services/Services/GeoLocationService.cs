using GeoLocatorApiHub.Infrastructure.Entities;
using GeoLocatorApiHub.Infrastructure.Repositories;
using GeoLocatorApiHub.Models;
using GeoLocatorApiHub.Services.Interfaces;
using Mapster;
using System.Net;
using System.Text.Json;

namespace GeoLocatorApiHub.Services.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IGeoLocationRepository _geoLocationRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public GeoLocationService(IGeoLocationRepository geoLocationRepository, IHttpClientFactory httpClientFactory)
        {
            _geoLocationRepository = geoLocationRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<GeoLocationDto>> GetAllGeoLocationsByIpAsync(string ip)
        {
            var geoLocationsResult = await _geoLocationRepository.GetGeoLocationsByIpAsync(ip);
            if (geoLocationsResult.IsSuccess)
            {
                return geoLocationsResult.Data.Adapt<IEnumerable<GeoLocationDto>>();
            }

            return new List<GeoLocationDto>();
        }

        public async Task<GeoLocationDto> GetLatestGeoLocationByIpAsync(string ip)
        {
            var latestGeoLocationResult = await _geoLocationRepository.GetLatestGeoLocationByIpAsync(ip);
            if (latestGeoLocationResult.IsSuccess)
            {
                return latestGeoLocationResult.Data.Adapt<GeoLocationDto>();
            }

            throw new Exception("GeoLocation not found for the provided IP.");
        }

        public async Task AddGeoLocationAsync(string ipOrUrl, string? apiKey)
        {
            string url = BuildUrlForGeoLocation(ipOrUrl, apiKey);

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var geoLocationData = JsonSerializer.Deserialize<ApiIpstackResponseJson>(jsonResponse);

                if (geoLocationData != null)
                {
                    var geoLocation = geoLocationData.Adapt<GeoLocation>();
                    await _geoLocationRepository.AddGeoLocationAsync(geoLocation);
                }
                else
                {
                    throw new InvalidOperationException("Failed to deserialize the data from the external API.");
                }
            }
            else
            {
                throw new Exception("Error retrieving data from ipstack.");
            }
        }

        public async Task<bool> DeleteGeoLocationAsync(string ip)
        {
            var deleteResult = await _geoLocationRepository.DeleteGeoLocationAsync(ip);
            return deleteResult.IsSuccess;
        }

        private string BuildUrlForGeoLocation(string ip, string? apiKey)
        {
            return @$"http://api.ipstack.com/{ip}?access_key={apiKey}&format=1";
        }
    }
}
