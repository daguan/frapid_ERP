using System;

namespace Frapid.Authentication.Exceptions
{
    public class EmailConfirmException : Exception
    {
        public EmailConfirmException(string message) : base(message)
        {
        }
    }
}