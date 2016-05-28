using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Installer.DAL;

namespace Frapid.Installer
{
    public class DbInspector
    {
        public DbInspector(string tenant, string database)
        {
            this.Tenant = tenant;
            this.Database = database;
        }

        public string Tenant { get; }
        public string Database { get; }

        public async Task<bool> HasDbAsync()
        {
            return await Store.HasDbAsync(this.Tenant, this.Database);
        }

        public bool IsWellKnownDb()
        {
            var serializer = new DomainSerializer("domains_approved.json");
            var domains = serializer.Get();
            return domains.Any(domain => TenantConvention.GetTenant(domain.DomainName) == this.Tenant);
        }
    }
}