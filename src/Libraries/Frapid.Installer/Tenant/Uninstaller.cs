using System.IO;
using System.Threading.Tasks;
using System.Web;
using Frapid.Configuration;
using Frapid.Framework;
using Frapid.Installer.DAL;

namespace Frapid.Installer.Tenant
{
    public class Uninstaller
    {
        public Uninstaller(string url)
        {
            this.Url = url;
            this.Tenant = TenantConvention.GetTenant(this.Url);
        }

        public string Url { get; set; }
        public string Tenant { get; set; }

        public async Task UnInstallAsync()
        {
            var context = FrapidHttpContext.GetCurrent();

            if (context != null)
            {
                throw new UninstallException("Access is denied. Deleting a website is not allowed.");
            }

            await this.CleanupDbAsync();
            this.CleanupTenantDirectory();
            new DomainSerializer("domains_approved.json").Remove(this.Url);
        }

        private void CleanupTenantDirectory()
        {
            var pathToTenant = PathMapper.MapPath($"/Tenants/{this.Tenant}");

            if (Directory.Exists(pathToTenant))
            {
                Directory.Delete(pathToTenant, true);
            }
        }

        private async Task CleanupDbAsync()
        {
            await Store.CleanupDbAsync(this.Tenant, this.Tenant);
        }
    }
}