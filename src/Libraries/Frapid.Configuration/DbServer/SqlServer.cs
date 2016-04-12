using System.Data.SqlClient;
using Frapid.Framework.Extensions;

namespace Frapid.Configuration.DbServer
{
    public class SqlServer : IDbServer
    {
        public string GetConnectionString(string database = "", string userId = "", string password = "")
        {
            string host = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Server");

            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "UserId");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Password");
            }

            bool enablePooling =
                ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "EnablePooling")
                    .ToUpperInvariant()
                    .Equals("TRUE");
            int port = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Port").To<int>();
            int minPoolSize =
                ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MinPoolSize").To<int>();
            int maxPoolSize =
                ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MaxPoolSize").To<int>();

            return this.GetConnectionString(host, database, userId, password, port, enablePooling, minPoolSize,
                maxPoolSize);
        }

        public string GetReportUserConnectionString(string database = "")
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            string userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ReportUserId");
            string password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation",
                "ReportUserPassword");

            return this.GetConnectionString(database, userId, password);
        }

        public string ProviderName => "System.Data.SqlClient";

        public string GetSuperUserConnectionString(string database = "")
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            string userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "SuperUserId");
            string password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation",
                "SuperUserPassword");

            return this.GetConnectionString(database, userId, password);
        }

        public string GetMetaConnectionString()
        {
            string database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");
            return this.GetConnectionString(database);
        }

        public string GetConnectionString(string host, string database, string username, string password, int port,
            bool enablePooling = true, int minPoolSize = 0, int maxPoolSize = 100)
        {
            string dataSource = host;

            if (port > 0)
            {
                dataSource += ", " + port;
            }

            return new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = database,
                UserID = username,
                Password = password,
                Pooling = enablePooling,
                MinPoolSize = minPoolSize,
                MaxPoolSize = maxPoolSize,
                ApplicationName = "Frapid"
            }.ConnectionString;
        }

        public string GetProcedureCommand(string procedureName, string[] parameters)
        {
            string sql = $"; EXECUTE {procedureName} {string.Join(", ", parameters)};";
            return sql;
        }

        public string DefaultSchemaQualify(string input)
        {
            return "[dbo]." + input;
        }

        public string AddLimit(string limit)
        {
            return $" FETCH NEXT {limit} ROWS ONLY";
        }

        public string AddOffset(string offset)
        {
            return $" OFFSET {offset} ROWS";
        }
    }
}