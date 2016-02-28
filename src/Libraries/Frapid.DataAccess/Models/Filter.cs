using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.Framework;
using Newtonsoft.Json.Linq;

namespace Frapid.DataAccess.Models
{
    public sealed class Filter : IPoco
    {
        public long FilterId { get; set; }
        public string ObjectName { get; set; }
        public string FilterName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public string FilterStatement { get; set; }
        public string ColumnName { get; set; }

        [Obsolete]
        public string PropertyName { get; set; }

        public int FilterCondition { get; set; }
        public string FilterValue { get; set; }
        public string FilterAndValue { get; set; }

        public static List<Filter> FromJArray(JArray array)
        {
            var filters = new List<Filter>();

            if (array == null)
            {
                return filters;
            }

            if (!array.Any())
            {
                return filters;
            }

            foreach (var element in array.Children<JObject>())
            {
                var filter = new Filter
                {
                    FilterId = element.TryGetPropertyValue<long>("filter_id"),
                    ObjectName = element.TryGetPropertyValue<string>("object_name"),
                    FilterName = element.TryGetPropertyValue<string>("filter_name"),
                    IsDefault = element.TryGetPropertyValue<bool>("is_default"),
                    IsDefaultAdmin = element.TryGetPropertyValue<bool>("is_default_admin"),
                    FilterStatement = element.TryGetPropertyValue<string>("filter_statement"),
                    ColumnName = element.TryGetPropertyValue<string>("column_name"),
                    PropertyName = element.TryGetPropertyValue<string>("property_name"),
                    FilterCondition = element.TryGetPropertyValue<int>("filter_condition"),
                    FilterValue = element.TryGetPropertyValue<string>("filter_value"),
                    FilterAndValue = element.TryGetPropertyValue<string>("filter_and_value")
                };

                filters.Add(filter);
            }

            return filters;
        }

    }
}