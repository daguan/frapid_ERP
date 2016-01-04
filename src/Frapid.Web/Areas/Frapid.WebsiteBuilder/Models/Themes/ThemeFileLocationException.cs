using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeFileLocationException : Exception
    {
        public ThemeFileLocationException()
        {
        }

        public ThemeFileLocationException(string message) : base(message)
        {
        }

        public ThemeFileLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}