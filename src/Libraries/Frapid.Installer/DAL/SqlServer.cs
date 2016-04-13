using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Serilog;

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

            using (var db = DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(tenant, database), tenant).GetDatabase())
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


            Log.Verbose($"Running SQL {sql}");

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