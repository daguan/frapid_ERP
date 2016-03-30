using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using static System.String;

namespace Frapid.WebsiteBuilder
{
    public class Configuration
    {
        private const string Path = "~/Tenants/{0}/Areas/Frapid.WebsiteBuilder/";
        private const string ConfigFile = "WebsiteBuilder.config";
        private const string DefaultThemeKey = "DefaultTheme";

        public static string GetCurrentThemePath()
        {
            string tenant = AppUsers.GetTenant();
            string path = Path + "Themes/{1}/";
            string theme = GetDefaultTheme();

            return Format(CultureInfo.InvariantCulture, path, tenant, theme);
        }

        public static string GetThemeDirectory()
        {
            string tenant = AppUsers.GetTenant();
            string path = Path + "Themes";

            return Format(CultureInfo.InvariantCulture, path, tenant);
        }

        public static string GetWebsiteBuilderPath()
        {
            string tenant = AppUsers.GetTenant();
            string path = HostingEnvironment.MapPath(Format(CultureInfo.InvariantCulture, Path, tenant));

            return path != null && !Directory.Exists(path) ? Empty : path;
        }

        public static string GetDefaultTheme()
        {
            return Get(DefaultThemeKey);
        }

        public static string Get(string key)
        {
            string path = GetWebsiteBuilderPath() + "/" + ConfigFile;
            return ConfigurationManager.ReadConfigurationValue(path, key);
        }
    }
}