using System;

namespace Frapid.Authentication.Exceptions
{
    public class PasswordConfirmException : Exception
    {
        public PasswordConfirmException(string message) : base(message)
        {
        }
    }
}