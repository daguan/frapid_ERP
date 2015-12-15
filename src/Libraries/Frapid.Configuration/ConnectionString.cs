using Npgsql;
using Frapid.Framework.Extensions;

namespace Frapid.Configuration
{
    public static class ConnectionString
    {
        public static string GetConnectionString(string database = "", string userId = "", string password = "")
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

            int port = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Port").To<int>();

            return GetConnectionString(host, database, userId, password, port);
        }

        public static string GetSuperUserConnectionString(string database = "")
        {
            if (string.IsNullOrWhiteSpace(database))
            {
                database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "Database");
            }

            string userId = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "SuperUserId");
            string password = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "SuperUserPassword");
            
            return GetConnectionString(database, userId, password);
        }

        public static string GetMetaConnectionString()
        {
            string database = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");
            return GetConnectionString(database);
        }

        private static string GetConnectionString(string host, string database, string username, string password, int port)
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Database = database,
                UserName = username,
                Password = password,
                Port = port,
                Pooling = true,
                SSL = true,
                SslMode = SslMode.Prefer,
                MinPoolSize = 10,
                MaxPoolSize = 100,
                ApplicationName = "Frapid"
            }.ConnectionString;
        }
    }
}