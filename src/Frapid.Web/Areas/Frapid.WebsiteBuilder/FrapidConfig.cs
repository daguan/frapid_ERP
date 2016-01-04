using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder
{
    public static class FrapidConfig
    {
        public static string[] GetMyAllowedResources(string catalog)
        {
            return Get(catalog, "MyAllowedResources").Split(',');
        }

        public static string[] GetAllowedUploadExtensions(string catalog)
        {
            return Get(catalog, "AllowedUploadExtensions").Split(',');
        }

        private static string Get(string catalog, string key)
        {
            string configFile = HostingEnvironment.MapPath($"~/Catalogs/{catalog}/Configs/Frapid.config");

            return !File.Exists(configFile) ? string.Empty : ConfigurationManager.ReadConfigurationValue(configFile, key);
        }
    }
}