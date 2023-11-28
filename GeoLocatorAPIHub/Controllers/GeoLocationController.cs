using GeoLocatorApiHub.Services.Interfaces;
using GeoLocatorApiHub.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GeoLocatorAPIHub.Controllers
{
    [ApiController]
    [Route("api/geolocations")]
    public class GeoLocationController : ControllerBase
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly MyExternalService _externalService;

        public GeoLocationController(IGeoLocationService geoLocationService, MyExternalService externalService)
        {
            _geoLocationService = geoLocationService;
            _externalService = externalService;
        }

        [HttpGet("{ipOrUrl}/latestGeoLocation")]
        public async Task<IActionResult> GetLatestGeoLocation(string ipOrUrl)
        {
            try
            {
                var geoLocationDto = await _geoLocationService.GetLatestGeoLocationByIpAsync(ipOrUrl);
                if (geoLocationDto == null)
                {
                    return NotFound($"Latest GeoLocation data for IP or URL '{ipOrUrl}' not found.");
                }
                return Ok(geoLocationDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{ip}")]
        public async Task<IActionResult> GetAllGeoLocations(string ip)
        {
            try
            {
                var geoLocations = await _geoLocationService.GetAllGeoLocationsByIpAsync(ip);
                return Ok(geoLocations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{ip}")]
        public async Task<IActionResult> AddGeoLocation(string ip)
        {
            if (!IsValidIpAddress(ip))
            {
                return BadRequest("Provided string is not a valid IPv4 or IPv6 address.");
            }

            try
            {
                await _geoLocationService.AddGeoLocationAsync(ip, _externalService.apiKey);
                return CreatedAtAction(nameof(GetLatestGeoLocation), new { ipOrUrl = ip }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ip}")]
        public async Task<IActionResult> DeleteGeoLocation(string ip)
        {
            try
            {
                var result = await _geoLocationService.DeleteGeoLocationAsync(ip);
                if (!result)
                {
                    return NotFound($"GeoLocation data for IP '{ip}' not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsValidIpAddress(string ipOrUrl)
        {
            return IPAddress.TryParse(ipOrUrl, out _);
        }
    }
}