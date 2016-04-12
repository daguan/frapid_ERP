using System.Linq;
using Frapid.Configuration;
using Frapid.Installer.DAL;

namespace Frapid.Installer
{
    public class DbInspector
    {
        public DbInspector(string database)
        {
            this.Database = database;
        }

        public string Database { get; }

        public bool HasDb()
        {
            return Store.HasDb(this.Database);
        }

        public bool IsWellKnownDb()
        {
            var serializer = new DomainSerializer("DomainsApproved.json");
            var domains = serializer.Get();
            return domains.Any(domain => DbConvention.GetTenant(domain.DomainName) == this.Database);
        }
    }
}