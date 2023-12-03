using GeoLocatorApiHub.Common.Exceptions;
using System.Net;

namespace GeoLocatorApiHub.Common.Validation
{
    public class IpAddressValidator
    {
        public void Validate(string ipAddresses)
        {
            var ips = ipAddresses.Split(',');

            if (ips.Length > 1)
            {
                throw new BatchNotSupportedException("Batch processing is not supported on this plan.");
            }

            foreach (var ip in ips)
            {
                if (!IsValidIpAddress(ip.Trim()))
                {
                    throw new InvalidIpAddressException("Provided string is not a valid IPv4 or IPv6 address.");
                }
            }
        }

        private bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }
    }
}
