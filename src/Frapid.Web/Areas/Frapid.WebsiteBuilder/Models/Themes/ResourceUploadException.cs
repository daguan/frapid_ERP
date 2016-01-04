using System;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ResourceUploadException : Exception
    {
        public ResourceUploadException()
        {
        }

        public ResourceUploadException(string message) : base(message)
        {
        }

        public ResourceUploadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}