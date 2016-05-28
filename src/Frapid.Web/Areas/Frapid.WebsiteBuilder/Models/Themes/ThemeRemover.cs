using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.Models.Themes
{
    public sealed class ThemeRemover
    {
        public ThemeRemover(string themeName)
        {
            this.ThemeName = themeName;
        }

        public string ThemeName { get; }

        public async Task RemoveAsync()
        {
            if(string.IsNullOrWhiteSpace(this.ThemeName))
            {
                await Task.Delay(10000);
                throw new ThemeRemoveException("Access is denied.");
            }

            string defaultTheme = Configuration.GetDefaultTheme();

            if(this.ThemeName.ToLower().Equals(defaultTheme.ToLower()))
            {
                throw new ThemeRemoveException("Access is denied. You cannot remove this theme because it is in use.");
            }

            string tenant = TenantConvention.GetTenant();
            string path = $"~/Tenants/{tenant}/Areas/Frapid.WebsiteBuilder/Themes/{this.ThemeName}";
            path = HostingEnvironment.MapPath(path);

            if(path == null ||
               !Directory.Exists(path))
            {
                throw new ThemeRemoveException("Invalid theme.");
            }

            Directory.Delete(path, true);
        }
    }
}