using System.Globalization;
using Frapid.Configuration;
using Frapid.DataAccess;
using Npgsql;

namespace Frapid.Installer
{
    public sealed class DbInstaller
    {
        public DbInstaller(string catalog)
        {
            this.Catalog = catalog;
        }

        public string Catalog { get; }

        public bool Install()
        {
            var inspector = new DbInspector(this.Catalog);
            bool hasDb = inspector.HasDb();
            bool canInstall = inspector.IsWellKnownDb();

            if (!hasDb && canInstall)
            {
                return this.CreateDb();
            }

            return false;
        }

        private bool CreateDb()
        {
            string sql = "CREATE DATABASE {0} WITH ENCODING='UTF8' TEMPLATE=template0 LC_COLLATE='C' LC_CTYPE='C';";
            sql = string.Format(CultureInfo.InvariantCulture, sql, Sanitizer.SanitizeIdentifierName(this.Catalog.ToLower()));

            string catalog = Factory.MetaDatabase;
            string connectionString = ConnectionString.GetSuperUserConnectionString(catalog);

            using (var command = new NpgsqlCommand(sql))
            {
                return DbOperation.ExecuteNonQuery(this.Catalog, command, connectionString);
            }
        }
    }
}