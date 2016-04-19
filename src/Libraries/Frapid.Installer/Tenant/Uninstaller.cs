using System.IO;
using System.Web.Hosting;
using Frapid.Configuration;
using Frapid.Installer.DAL;

namespace Frapid.Installer.Tenant
{
    public class Uninstaller
    {
        public Uninstaller(string url)
        {
            this.Url = url;
            this.Tenant = DbConvention.GetTenant(this.Url);
        }

        public string Url { get; set; }
        public string Tenant { get; set; }

        public void UnInstall()
        {
            if (HostingEnvironment.IsHosted)
            {
                throw new UninstallException("Access is denied. Deleting a website is not allowed.");
            }

            this.CleanupDb();
            this.CleanupTenantDirectory();
            new DomainSerializer("DomainsApproved.json").Remove(this.Url);
        }

        private void CleanupTenantDirectory()
        {
            string pathToTenant = PathMapper.MapPath($"/Tenants/{this.Tenant}");

            if (Directory.Exists(pathToTenant))
            {
                Directory.Delete(pathToTenant, true);
            }
        }

        private void CleanupDb()
        {
            Store.CleanupDb(this.Tenant, this.Tenant);
        }
    }
}