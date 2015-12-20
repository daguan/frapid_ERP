using System.Globalization;
using System.IO;
using System.Web.Hosting;
using Frapid.ApplicationState.Cache;

namespace Frapid.Account
{
    public class Configuration
    {
        private const string Path = "~/Catalogs/{0}/Areas/Frapid.Account/";

        public static string GetOverridePath()
        {
            string catalog = AppUsers.GetCatalog();
            string path = HostingEnvironment.MapPath(string.Format(CultureInfo.InvariantCulture, Path, catalog));

            return path != null && !Directory.Exists(path) ? string.Empty : path;
        }
    }
}