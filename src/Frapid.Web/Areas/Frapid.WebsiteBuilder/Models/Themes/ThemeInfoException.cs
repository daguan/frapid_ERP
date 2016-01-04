using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeInfoException : Exception
    {
        public ThemeInfoException()
        {
        }

        public ThemeInfoException(string message) : base(message)
        {
        }

        public ThemeInfoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}