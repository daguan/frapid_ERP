using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder
{
    public class ThemeConfiguration
    {
        private const string DefaultLayout = "DefaultLayout";
        private const string HomepageLayout = "HomepageLayout";
        private const string BlogLayout = "BlogLayout";

        public static string GetLayout(string theme)
        {
            return Get(theme, DefaultLayout);
        }

        public static string GetHomepageLayout(string theme)
        {
            return Get(theme, HomepageLayout);
        }

        public static string GetBlogLayout(string theme)
        {
            return Get(theme, BlogLayout);
        }

        public static string Get(string theme, string key)
        {
            string path = Configuration.GetCurrentThemePath() + "/Theme.config";
            path = HostingEnvironment.MapPath(path);

            return !File.Exists(path) ? string.Empty : ConfigurationManager.ReadConfigurationValue(path, key);
        }
    }
}