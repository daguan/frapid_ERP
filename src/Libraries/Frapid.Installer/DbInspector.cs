using System.Linq;
using Frapid.Configuration;
using Frapid.DataAccess;

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
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_database WHERE datname=@0;";
            string database = Factory.MetaDatabase;
            string connectionString = ConnectionString.GetSuperUserConnectionString(database);

            using (var db = DbProvider.Get(connectionString).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, this.Database).Equals(1);
            }
        }

        public bool IsWellKnownDb()
        {
            var serializer = new DomainSerializer("DomainsApproved.json");
            var domains = serializer.Get();
            return domains.Any(domain => DbConvention.GetTenant(domain.DomainName) == this.Database);
        }
    }
}