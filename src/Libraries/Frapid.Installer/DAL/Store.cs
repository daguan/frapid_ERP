using System;
using System.Linq;
using Frapid.Configuration;
using Serilog;

namespace Frapid.Installer.DAL
{
    public static class Store
    {
        private static IStore GetDbServer(string tenant)
        {
            var site = DbConvention.GetSite(tenant);
            string providerName = site.DbProvider;

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
            GetDbServer(tenant).CreateDb(tenant);
        }

        public static bool HasDb(string tenant, string dbName)
        {
            return GetDbServer(tenant).HasDb(tenant, dbName);
        }

        public static bool HasSchema(string tenant, string database, string schema)
        {
            return GetDbServer(tenant).HasSchema(tenant, database, schema);
        }

        public static void RunSql(string tenant, string database, string fromFile)
        {
            GetDbServer(tenant).RunSql(tenant, database, fromFile);
        }
    }
}