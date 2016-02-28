using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frapid.Framework
{
    public static class JsonHelper
    {
        public static T TryGetPropertyValue<T>(this JObject element, string key)
        {
            JToken value;
            element.TryGetValue(key, out value);

            if (value == null)
            {
                return default(T);
            }

            return value.ToObject<T>();
        }

        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                Culture = CultureInfo.DefaultThreadCurrentCulture,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateParseHandling = DateParseHandling.DateTime,
                FloatParseHandling = FloatParseHandling.Decimal,
                FloatFormatHandling = FloatFormatHandling.DefaultValue
            };
        }

        public static JsonSerializer GetJsonSerializer()
        {
            return JsonSerializer.Create(GetJsonSerializerSettings());
        }
    }
}