using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeCreateException : Exception
    {
        public ThemeCreateException()
        {
        }

        public ThemeCreateException(string message) : base(message)
        {
        }

        public ThemeCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}