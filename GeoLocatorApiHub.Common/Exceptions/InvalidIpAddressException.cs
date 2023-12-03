namespace GeoLocatorApiHub.Common.Exceptions
{
    public class InvalidIpAddressException : Exception
    {
        public InvalidIpAddressException(string message) : base(message) { }
    }
}
