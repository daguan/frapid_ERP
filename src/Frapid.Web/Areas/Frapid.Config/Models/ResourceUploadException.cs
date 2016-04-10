using System;

namespace Frapid.Config.Models
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