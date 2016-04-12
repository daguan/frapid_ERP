using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using Frapid.Configuration;
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

            string database = Factory.MetaDatabase;
            string connectionString = FrapidDbServer.GetSuperUserConnectionString(database);
            Factory.Execute(connectionString, sql);
        }

        public bool HasDb(string dbName)
        {
            const string sql = "SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name=@0;";
            string database = Factory.MetaDatabase;
            string connectionString = FrapidDbServer.GetSuperUserConnectionString(database);

            using (var db = DbProvider.Get(connectionString).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, dbName).Equals(1);
            }
        }

        public bool HasSchema(string database, string schema)
        {
            const string sql = "SELECT COUNT(*) FROM sys.schemas WHERE name=@0;";

            using (var db = DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(database)).GetDatabase())
            {
                return db.ExecuteScalar<int>(sql, schema).Equals(1);
            }
        }

        public void RunSql(string database, string fromFile)
        {
            fromFile = fromFile.Replace("{DbServer}", "SQL Server");
            if (string.IsNullOrWhiteSpace(fromFile) || File.Exists(fromFile).Equals(false))
            {
                return;
            }

            string sql = File.ReadAllText(fromFile, Encoding.UTF8);


            Log.Verbose($"Running SQL {sql}");

            string connectionString = FrapidDbServer.GetSuperUserConnectionString(database);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var svrConnection = new ServerConnection(sqlConnection);
                var server = new Server(svrConnection);
                server.ConnectionContext.ExecuteNonQuery(sql);
            }
        }
    }
}