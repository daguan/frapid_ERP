using System.Linq;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Installer
{
    public class DbInspector
    {
        public DbInspector(string catalog)
        {
            this.Catalog = catalog;
        }

        public string Catalog { get; }

        public bool HasDb()
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_database WHERE datname=@0;";
            return Factory.Scalar<int>(Factory.MetaDatabase, sql, this.Catalog).Equals(1);
        }

        public bool IsWellKnownDb()
        {
            var serializer = new DomainSerializer("domains-approved.json");
            var domains = serializer.Get();
            return domains.Any(domain => DbConvention.GetCatalog(domain.DomainName) == this.Catalog);
        }
    }
}