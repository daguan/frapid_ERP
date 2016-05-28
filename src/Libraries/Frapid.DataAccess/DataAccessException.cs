using System;
using System.ComponentModel;

namespace Frapid.DataAccess
{
    public class DataAccessException: Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException([Localizable(true)] string message): base(message)
        {
        }

        public DataAccessException([Localizable(true)] string message, Exception exception): base(message, exception)
        {
        }
    }
}