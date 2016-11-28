using System;
using System.Data.SqlClient;

namespace Frapid.DataAccess.Extensions
{
    public static class SqlClientExtensions
    {
        public static SqlParameter AddWithNullableValue(this SqlParameterCollection collection, string parameterName, object value)
        {
            return collection.AddWithValue(parameterName, value ?? DBNull.Value);
        }
    }
}