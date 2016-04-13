using System.Globalization;
using System.IO;
using System.Text;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Serilog;

namespace Frapid.Installer.DAL
{
    public sealed class PostgreSQL : IStore
    {
        public string ProviderName { get; } = "Npgsql";

        public void CreateDb(string tenant)
        {
            string sql = "CREATE DATABASE {0} WITH ENCODING='UTF8' TEMPLATE=template0 LC_COLLATE='C' LC_CTYPE='C';";
            sql = string.Format(CultureInfo.InvariantCulture, sql, Sanitizer.SanitizeIdentifierName(tenant.ToLower()));

            string database = Factory.GetMetaDatabase(tenant);
            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            Factory.Execute(connectionString, tenant, sql);
        }

        public bool HasDb(string tenant, string database)
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_database WHERE datname=@0;";

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var db = DbProvider.Get(connectionString, tenant).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, tenant).Equals(1);
            }
        }

        public bool HasSchema(string tenant, string database, string schema)
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_namespace WHERE nspname=@0;";

            using (var db = DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(tenant, database), tenant).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, schema).Equals(1);
            }
        }

        public void RunSql(string tenant, string database, string fromFile)
        {
            fromFile = fromFile.Replace("{DbServer}", "PostgreSQL");
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile, Encoding.UTF8);

            //PetaPoco/NPoco Escape
            //ORM: Remove this behavior if you change the ORM.
            sql = sql.Replace("@", "@@");

            Log.Verbose($"Running SQL {sql}");

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            Factory.Execute(connectionString, database, sql);
        }
    }
}