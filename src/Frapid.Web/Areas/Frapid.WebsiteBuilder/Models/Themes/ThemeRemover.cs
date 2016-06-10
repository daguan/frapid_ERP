using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeRemover
    {
        public ThemeRemover(string tenant, string themeName)
        {
            this.Tenant = tenant;
            this.ThemeName = themeName;
        }

        public string Tenant { get; set; }
        public string ThemeName { get; }

        public async Task RemoveAsync()
        {
            if (string.IsNullOrWhiteSpace(this.ThemeName))
            {
                await Task.Delay(10000).ConfigureAwait(false);
                throw new ThemeRemoveException("Access is denied.");
            }

            string defaultTheme = Configuration.GetDefaultTheme(this.Tenant);

            if (this.ThemeName.ToLower().Equals(defaultTheme.ToLower()))
            {
                throw new ThemeRemoveException("Access is denied. You cannot remove this theme because it is in use.");
            }

            string path = $"~/Tenants/{this.Tenant}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeName}";
            path = HostingEnvironment.MapPath(path);

            if (path == null ||
                !Directory.Exists(path))
            {
                throw new ThemeRemoveException("Invalid theme.");
            }

            Directory.Delete(path, true);
        }
    }
}