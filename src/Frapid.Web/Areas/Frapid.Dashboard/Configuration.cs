using System.Globalization;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using static System.String;

namespace Frapid.Dashboard
{
    public class Configuration
    {
        private const string Path = "~/Catalogs/{0}/Areas/Frapid.Dashboard/";
        private const string ConfigFile = "Dashboard.config";
        private const string DefaultThemeKey = "DefaultTheme";

        public static string GetCurrentThemePath()
        {
            string catalog = AppUsers.GetCatalog();
            string path = Path + "Themes/{1}/";
            string theme = GetDefaultTheme();

            return Format(CultureInfo.InvariantCulture, path, catalog, theme);
        }

        public static string GetDashboardPath()
        {
            string catalog = AppUsers.GetCatalog();
            string path = HostingEnvironment.MapPath(Format(CultureInfo.InvariantCulture, Path, catalog));

            return path != null && !System.IO.Directory.Exists(path) ? Empty : path;
        }

        public static string GetDefaultTheme()
        {
            return Get(DefaultThemeKey);
        }

        public static string Get(string key)
        {
            string path = GetDashboardPath() + "/" + ConfigFile;
            return ConfigurationManager.ReadConfigurationValue(path, key);
        }
    }
}