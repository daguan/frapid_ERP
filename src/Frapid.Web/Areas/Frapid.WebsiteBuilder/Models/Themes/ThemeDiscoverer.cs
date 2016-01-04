using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Frapid.Configuration;
using Serilog;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeDiscoverer
    {
        public List<string> Discover()
        {
            string catalog = DbConvention.GetCatalog();
            string path = $"~/Catalogs/{catalog}/Areas/Frapid.WebsiteBuilder/Themes";
            path = HostingEnvironment.MapPath(path);

            if (path == null || !Directory.Exists(path))
            {
                Log.Warning("Could not discover theme(s) on path {path}.", path);
                throw new ThemeDiscoveryException(
                    "Cannot find the theme directory. Check application logs for more information.");
            }

            var directories = Directory.GetDirectories(path);
            return directories.Select(directory => new DirectoryInfo(directory).Name).ToList();
        }
    }
}