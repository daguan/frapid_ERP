using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeUploadException : Exception
    {
        public ThemeUploadException()
        {
        }

        public ThemeUploadException(string message) : base(message)
        {
        }

        public ThemeUploadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}