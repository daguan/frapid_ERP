using System;

namespace Frapid.Areas.Conventions.Attachments
{
    public sealed class UploadException : Exception
    {
        public UploadException()
        {
        }

        public UploadException(string message) : base(message)
        {
        }

        public UploadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}