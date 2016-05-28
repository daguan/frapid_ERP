using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.Installer.Helpers;
using Frapid.NPoco;

namespace Frapid.Installer.DAL
{
    public sealed class SqlServer : IStore
    {
        public string ProviderName { get; } = "System.Data.SqlClient";

        public async Task CreateDbAsync(string tenant)
        {
            var sql = "CREATE DATABASE [{0}];";
            sql = string.Format(CultureInfo.InvariantCulture, sql, Sanitizer.SanitizeIdentifierName(tenant.ToLower()));

            var database = Factory.GetMetaDatabase(tenant);
            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            await Factory.ExecuteAsync(connectionString, tenant, sql);
        }

        public async Task<bool> HasDbAsync(string tenant, string database)
        {
            const string sql = "SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name=@0;";

            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var db = DbProvider.Get(connectionString, tenant).GetDatabase())
            {
                var awaiter = await db.ExecuteScalarAsync<int>(sql, new object[] { tenant });
                return awaiter.Equals(1);
            }
        }

        public async Task<bool> HasSchemaAsync(string tenant, string database, string schema)
        {
            const string sql = "SELECT COUNT(*) FROM sys.schemas WHERE name=@0;";

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
            fromFile = fromFile.Replace("{DbServer}", "SQL Server");
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            var sql = File.ReadAllText(fromFile, Encoding.UTF8);


            InstallerLog.Verbose($"Running file {fromFile}");

            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var connection = new SqlConnection(connectionString))
            {
                await this.RunScriptAsync(connection, sql);
            }
        }

        public async Task CleanupDbAsync(string tenant, string database)
        {
            var sql = @"DECLARE @sql nvarchar(MAX);
                            DECLARE @queries TABLE(id int identity, query nvarchar(500), done bit DEFAULT(0));
                            DECLARE @id int;
                            DECLARE @query nvarchar(500);


                            INSERT INTO @queries(query)
                            SELECT 
	                            'EXECUTE dbo.drop_schema ''' + sys.schemas.name + ''''+ CHAR(13) AS query
                            FROM sys.schemas
                            WHERE principal_id = 1
                            AND name != 'dbo'
                            ORDER BY schema_id;



                            WHILE(SELECT COUNT(*) FROM @queries WHERE done = 0) > 0
                            BEGIN
                                SELECT TOP 1 
		                            @id = id,
		                            @query = query
	                            FROM @queries 
	                            WHERE done = 0
	
	                            EXECUTE(@query);

                                UPDATE @queries SET done = 1 WHERE id=@id;
                            END;";

            var connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task RunScriptAsync(SqlConnection connection, string sql)
        {
            var regex = new Regex(@"\r{0,1}\nGO\r{0,1}\n");
            var commands = regex.Split(sql);

            foreach (var item in commands)
            {
                if (item != string.Empty)
                {
                    using (var command = new SqlCommand(item, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}