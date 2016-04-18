using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.Installer.Helpers;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Frapid.Installer.DAL
{
    public sealed class SqlServer : IStore
    {
        public string ProviderName { get; } = "System.Data.SqlClient";


        public void CreateDb(string tenant)
        {
            string sql = "CREATE DATABASE [{0}];";
            sql = string.Format(CultureInfo.InvariantCulture, sql, Sanitizer.SanitizeIdentifierName(tenant.ToLower()));

            string database = Factory.GetMetaDatabase(tenant);
            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);
            Factory.Execute(connectionString, tenant, sql);
        }

        public bool HasDb(string tenant, string database)
        {
            const string sql = "SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name=@0;";

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var db = DbProvider.Get(connectionString, tenant).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, tenant).Equals(1);
            }
        }

        public bool HasSchema(string tenant, string database, string schema)
        {
            const string sql = "SELECT COUNT(*) FROM sys.schemas WHERE name=@0;";

            using (
                var db =
                    DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(tenant, database), tenant).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, schema).Equals(1);
            }
        }

        public void RunSql(string tenant, string database, string fromFile)
        {
            fromFile = fromFile.Replace("{DbServer}", "SQL Server");
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile, Encoding.UTF8);


            InstallerLog.Verbose($"Running file {fromFile}");

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var svrConnection = new ServerConnection(sqlConnection);
                var server = new Server(svrConnection);
                server.ConnectionContext.ExecuteNonQuery(sql);
            }
        }

        public void CleanupDb(string tenant, string database)
        {
            string sql = @"DECLARE @sql nvarchar(MAX);
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

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(tenant, database);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var svrConnection = new ServerConnection(sqlConnection);
                var server = new Server(svrConnection);
                server.ConnectionContext.ExecuteNonQuery(sql);
            }
        }
    }
}