using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Installer.DAL
{
    public sealed class PostgreSQL : IStore
    {
        public string ProviderName { get; } = "Npgsql";

        public async Task CreateDbAsync(string tenant)
        {
            var sql = "CREATE DATABASE {0} WITH ENCODING='UTF8' TEMPLATE=template0 LC_COLLATE='C' LC_CTYPE='C';";
            sql = string.Format(CultureInfo.InvariantCulture, sql, Sanitizer.SanitizeIdentifierName(tenant.ToLower()));

            var database = Factory.GetMetaDatabase(tenant);
            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            await Factory.ExecuteAsync(connectionString, tenant, sql);
        }

        public async Task<bool> HasDbAsync(string tenant, string database)
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_database WHERE datname=@0;";

            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var db = DbProvider.Get(connectionString, tenant).GetDatabase())
            {
                var awaiter = await db.ExecuteScalarAsync<int>(sql, new object[] { tenant });
                return awaiter.Equals(1);
            }
        }

        public async Task<bool> HasSchemaAsync(string tenant, string database, string schema)
        {
            const string sql = "SELECT COUNT(*) FROM pg_catalog.pg_namespace WHERE nspname=@0;";

            using (
                var db =
                    DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(tenant, database), tenant).GetDatabase())
            {
                var awaiter = await db.ExecuteScalarAsync<int>(sql, new object[] { schema });
                return awaiter.Equals(1);
            }
        }

        public async Task RunSqlAsync(string tenant, string database, string fromFile)
        {
            fromFile = fromFile.Replace("{DbServer}", "PostgreSQL");
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            var sql = File.ReadAllText(fromFile, Encoding.UTF8);

            //PetaPoco/NPoco Escape
            //ORM: Remove this behavior if you change the ORM.
            sql = sql.Replace("@", "@@");


            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            await Factory.ExecuteAsync(connectionString, database, sql);
        }

        public async Task CleanupDbAsync(string tenant, string database)
        {
            var sql = @"DO
                            $$
                                DECLARE _schemas            text[];
                                DECLARE _schema             text;
                                DECLARE _sql                text;
                            BEGIN
                                SELECT 
                                    array_agg(nspname::text)
                                INTO _schemas
                                FROM pg_namespace
                                WHERE nspname NOT LIKE 'pg_%'
                                AND nspname NOT IN('information_schema', 'public');

                                IF(_schemas IS NOT NULL) THEN
                                    FOREACH _schema IN ARRAY _schemas
                                    LOOP
                                        _sql := 'DROP SCHEMA IF EXISTS ' || _schema || ' CASCADE;';

                                        EXECUTE _sql;
                                    END LOOP;
                                END IF;
                            END
                            $$
                            LANGUAGE plpgsql;";

            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            await Factory.ExecuteAsync(connectionString, database, sql);
        }
    }
}