using System;

namespace Frapid.Account.Exceptions
{
    public class PasswordConfirmException : Exception
    {
        public PasswordConfirmException(string message) : base(message)
        {
        }
    }
}