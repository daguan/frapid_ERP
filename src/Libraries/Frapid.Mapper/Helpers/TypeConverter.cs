using System;
using System.Linq;
using Frapid.Framework.Extensions;

namespace Frapid.Mapper.Helpers
{
    public static class TypeConverter
    {
        public static object Convert(object value, Type destType)
        {
            if (destType == typeof(string))
            {
                return value.ToString();
            }

            if (destType == typeof(bool))
            {
                if (value is string)
                {
                    return new[]
                    {
                    "TRUE",
                    "YES",
                    "T"
                    }.Contains(value.ToString().ToUpperInvariant());
                }
            }

            if (destType == typeof(object))
            {
                return value;
            }

            if (destType == typeof(DateTimeOffset) || destType == typeof(DateTimeOffset?))
            {
                if (value is DateTime)
                {
                    return new DateTimeOffset((DateTime)value);
                }

                return value.To<DateTime>();
            }

            if (destType == typeof(DateTime) || destType == typeof(DateTime?))
            {
                if (value is DateTimeOffset)
                {
                    return ((DateTimeOffset)value).DateTime;
                }

                return value.To<DateTime>();
            }

            return value;
        }
    }
}