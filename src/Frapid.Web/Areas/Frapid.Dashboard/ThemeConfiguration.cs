using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.Dashboard
{
    public class ThemeConfiguration
    {
        private const string LayoutFile = "LayoutFile";

        public static string GetLayout(string theme)
        {
            return Get(theme, LayoutFile);
        }

        public static string Get(string theme, string key)
        {
            string path = Configuration.GetCurrentThemePath() + "/Theme.config";
            path = HostingEnvironment.MapPath(path);

            return !File.Exists(path) ? string.Empty : ConfigurationManager.ReadConfigurationValue(path, key);
        }
    }
}