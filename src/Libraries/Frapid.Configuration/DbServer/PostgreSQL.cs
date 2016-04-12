using Frapid.Framework.Extensions;
using Npgsql;

namespace Frapid.Configuration.DbServer
{
    public class PostgreSQL : IDbServer
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

            bool enablePooling = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "EnablePooling").ToUpperInvariant().Equals("TRUE");
            int port = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Port").To<int>();
            int minPoolSize = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MinPoolSize").To<int>();
            int maxPoolSize = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MaxPoolSize").To<int>();

            return this.GetConnectionString(host, database, userId, password, port, enablePooling, minPoolSize, maxPoolSize);
        }

        public string GetReportUserConnectionString(string database = "")
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            string userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ReportUserId");
            string password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ReportUserPassword");

            return this.GetConnectionString(database, userId, password);
        }

        public string ProviderName => "Npgsql";

        public string GetSuperUserConnectionString(string database = "")
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            string userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "SuperUserId");
            string password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "SuperUserPassword");

            return this.GetConnectionString(database, userId, password);
        }

        public string GetMetaConnectionString()
        {
            string database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");
            return this.GetConnectionString(database);
        }

        public string GetConnectionString(string host, string database, string username, string password, int port, bool enablePooling = true, int minPoolSize = 0, int maxPoolSize = 100)
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Database = database,
                UserName = username,
                Password = password,
                Port = port,
                Pooling = enablePooling,
                SSL = true,
                SslMode = SslMode.Prefer,
                MinPoolSize = minPoolSize,
                MaxPoolSize = maxPoolSize,
                ApplicationName = "Frapid"
            }.ConnectionString;
        }

        public string GetProcedureCommand(string procedureName, string[] parameters)
        {
            string sql = $"SELECT * FROM {procedureName}({string.Join(", ", parameters)});";
            return sql;
        }

        public string DefaultSchemaQualify(string input)
        {
            return "public." + input;
        }

        public string AddLimit(string limit)
        {
            return $" LIMIT {limit}";
        }

        public string AddOffset(string offset)
        {
            return $" OFFSET {offset}";
        }
    }
}