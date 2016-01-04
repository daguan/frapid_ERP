using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeRemoveException : Exception
    {
        public ThemeRemoveException()
        {
        }

        public ThemeRemoveException(string message) : base(message)
        {
        }

        public ThemeRemoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}