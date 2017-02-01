using System.IO;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;

namespace Frapid.Calendar
{
    public static class Configs
    {
        public static string GoogleApiKey => Get("GoogleApiKey");

        public static string Get(string key)
        {
            string tenant = AppUsers.GetTenant();
            return Get(tenant, key);
        }

        public static string Get(string tenant, string key)
        {
            string configurationFile = $"/Tenants/{tenant}/Configs/Calendar.config";
            configurationFile = PathMapper.MapPath(configurationFile);

            if (!File.Exists(configurationFile))
            {
                return string.Empty;
            }

            return ConfigurationManager.ReadConfigurationValue(configurationFile, key);
        }
    }
}