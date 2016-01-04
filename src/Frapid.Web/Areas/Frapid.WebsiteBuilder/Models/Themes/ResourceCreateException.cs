using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ResourceCreateException : Exception
    {
        public ResourceCreateException()
        {
        }

        public ResourceCreateException(string message) : base(message)
        {
        }

        public ResourceCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}