using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Mapper.Helpers;

namespace Frapid.Mapper.Extensions
{
    public static class DynamicExtensions
    {
        public static ExpandoObject ToExpando(this IDictionary<string, object> source)
        {
            var retVal = new ExpandoObject();
            var dictionary = (IDictionary<string, object>) retVal;

            foreach (var keyValue in source)
            {
                if (keyValue.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>) keyValue.Value).ToExpando();
                    dictionary.Add(keyValue.Key, expandoValue);
                }
                else if (keyValue.Value is ICollection)
                {
                    var itemList = new List<object>();

                    foreach (var item in (ICollection) keyValue.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>) item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    dictionary.Add(keyValue.Key, itemList);
                }
                else
                {
                    dictionary.Add(keyValue);
                }
            }

            return retVal;
        }

        public static T FromDynamic<T>(this ExpandoObject dynamic)
        {
            var dictionary = new Dictionary<string, object>(dynamic);

            if (typeof(T) == typeof(object))
            {
                var eo = new ExpandoObject();
                var eoColl = (ICollection<KeyValuePair<string, object>>) eo;

                foreach (var kvp in dictionary)
                {
                    eoColl.Add(kvp);
                }

                dynamic eoDynamic = eo;
                return eoDynamic;
            }

            var item = Activator.CreateInstance<T>();


            foreach (var property in typeof(T).GetProperties().Where(x => x.CanWrite))
            {
                string key = dictionary.Keys.SingleOrDefault(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                var propertyValue = dictionary[key];

                if (propertyValue != null)
                {
                    property.SetValue(item, TypeConverter.Convert(propertyValue, property.PropertyType));
                }
            }

            return item;
        }
    }
}