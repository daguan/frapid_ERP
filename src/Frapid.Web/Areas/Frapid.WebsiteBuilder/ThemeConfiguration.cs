using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder
{
    public class ThemeConfiguration
    {
        private const string DefaultDocumentKey = "DefaultDocument";

        public static string GetDefaultDocument(string theme)
        {
            return Get(theme, DefaultDocumentKey);
        }

        public static string Get(string theme, string key)
        {
            string path = Configuration.GetCurrentThemePath() + "/Theme.config";
            path = HostingEnvironment.MapPath(path);

            return !File.Exists(path) ? string.Empty : ConfigurationManager.ReadConfigurationValue(path, key);
        }
    }
}