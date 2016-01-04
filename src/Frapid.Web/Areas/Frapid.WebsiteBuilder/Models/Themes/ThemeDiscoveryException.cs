using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeDiscoveryException : Exception
    {
        public ThemeDiscoveryException()
        {
        }

        public ThemeDiscoveryException(string message) : base(message)
        {
        }

        public ThemeDiscoveryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}