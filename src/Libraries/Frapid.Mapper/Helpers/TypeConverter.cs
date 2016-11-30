using System;

namespace Frapid.Mapper.Helpers
{
    public static class TypeConverter
    {
        public static object Convert(object value, Type destType)
        {
            if (value is DateTime)
            {
                if (destType == typeof(DateTimeOffset) ||
                    destType == typeof(DateTimeOffset?))
                {
                    //value is not null
                    return new DateTimeOffset((DateTime) value);
                }

                return value;
            }

            if (value is DateTimeOffset)
            {
                if (destType == typeof(DateTime) ||
                    destType == typeof(DateTime?))
                {
                    //value is not null
                    return ((DateTimeOffset) value).DateTime;
                }

                return (DateTimeOffset) value;
            }

            return value;
        }
    }
}