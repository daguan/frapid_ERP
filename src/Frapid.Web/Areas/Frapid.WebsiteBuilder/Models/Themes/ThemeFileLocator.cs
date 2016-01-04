using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeFileLocator
    {
        public ThemeFileLocator(string themeName, string file)
        {
            this.ThemeName = themeName;
            this.File = file;
        }

        public string ThemeName { get; }
        public string File { get; }

        public string Locate()
        {
            string catalog = DbConvention.GetCatalog();
            string path = $"~/Catalogs/{catalog}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeName}/{this.File}";
            path = HostingEnvironment.MapPath(path);

            if (!System.IO.File.Exists(path))
            {
                throw new ThemeFileLocationException("Could not locate the requested file.");
            }

            return path;
        }
    }
}