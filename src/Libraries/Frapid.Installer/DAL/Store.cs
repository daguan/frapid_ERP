using System;
using System.Linq;
using Frapid.Configuration;
using Serilog;

namespace Frapid.Installer.DAL
{
    public static class Store
    {
        private static readonly IStore DbServer = GetDbAbstraction();

        private static IStore GetDbAbstraction()
        {
            string providerName = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ProviderName");

            try
            {
                var iType = typeof(IStore);
                var members = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance);

                foreach (var member in members.Cast<IStore>().Where(member => member.ProviderName.Equals(providerName)))
                {
                    return member;
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex);
                throw;
            }

            return new PostgreSQL();
        }

        public static void CreateDb(string tenant)
        {
            DbServer.CreateDb(tenant);
        }

        public static bool HasDb(string dbName)
        {
            return DbServer.HasDb(dbName);
        }

        public static bool HasSchema(string database, string schema)
        {
            return DbServer.HasSchema(database, schema);
        }

        public static void RunSql(string database, string fromFile)
        {
            DbServer.RunSql(database, fromFile);
        }
    }
}