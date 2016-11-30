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
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();

            var expandoDic = (IDictionary<string, object>) expando;

            // go through the items in the dictionary and copy over the key value pairs)

            foreach (var kvp in dictionary)

            {
                // if the value can also be turned into an ExpandoObject, then do it!

                if (kvp.Value is IDictionary<string, object>)

                {
                    var expandoValue = ((IDictionary<string, object>) kvp.Value).ToExpando();

                    expandoDic.Add(kvp.Key, expandoValue);
                }

                else if (kvp.Value is ICollection)

                {
                    // iterate through the collection and convert any strin-object dictionaries

                    // along the way into expando objects

                    var itemList = new List<object>();

                    foreach (var item in (ICollection) kvp.Value)

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

                    expandoDic.Add(kvp.Key, itemList);
                }

                else

                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
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
                var key =
                    dictionary.Keys.SingleOrDefault(x => x.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

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