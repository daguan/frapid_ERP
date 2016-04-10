using System;

namespace Frapid.Config.Models
{
    public sealed class ResourceRemoveException : Exception
    {
        public ResourceRemoveException()
        {
        }

        public ResourceRemoveException(string message) : base(message)
        {
        }

        public ResourceRemoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}