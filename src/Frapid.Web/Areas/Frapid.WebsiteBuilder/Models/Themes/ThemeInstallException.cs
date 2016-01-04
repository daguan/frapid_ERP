using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeInstallException : Exception
    {
        public ThemeInstallException()
        {
        }

        public ThemeInstallException(string message) : base(message)
        {
        }

        public ThemeInstallException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}