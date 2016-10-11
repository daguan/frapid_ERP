using System;
using Frapid.Framework.Extensions;
using Frapid.Reports.Engine.Model;

namespace Frapid.Reports.Helpers
{
    public static class DataSourceParameterHelper
    {
        public static object CastValue(object value, DataSourceParameterType type)
        {
            switch (type)
            {
                case DataSourceParameterType.Date:
                    double milliseconds = value.To<double>();
                                
                    if (milliseconds > 0)
                    {
                        value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            .AddMilliseconds(milliseconds)
                            .ToLocalTime()
                            .Date
                            .ToString("d");
                    }
                    else
                    {
                        return value.To<DateTime>();
                    }
                    break;
                case DataSourceParameterType.Number:
                    return value.To<long>();
                case DataSourceParameterType.Bool:
                    return value.To<bool>();
                case DataSourceParameterType.Decimal:
                    return value.To<decimal>();
                case DataSourceParameterType.Double:
                    return value.To<double>();
            }

            return value;
        }
    }
}