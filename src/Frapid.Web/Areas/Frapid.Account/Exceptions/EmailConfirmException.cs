using System;

namespace Frapid.Account.Exceptions
{
    public class EmailConfirmException : Exception
    {
        public EmailConfirmException(string message) : base(message)
        {
        }
    }
}