using System.Collections.Generic;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.i18n.Models;

namespace Frapid.i18n.DAL
{
    public static class DbResources
    {
        public static Dictionary<string, string> GetLocalizedResources(string tenant)
        {
            const string sql = "SELECT * FROM i18n.localized_resource_view;";

            using (var db = DbProvider.Get(FrapidDbServer.GetMetaConnectionString(tenant), tenant).GetDatabase())
            {
                var dbResources = db.Query<dynamic>(sql);

                var resources = new Dictionary<string, string>();

                foreach (var resource in dbResources)
                {
                    string key = resource.Key;
                    string value = resource.Value;

                    resources.Add(key, value);
                }

                return resources;
            }
        }

        public static IEnumerable<LocalizedResource> GetLocalizationTable(string tenant, string language)
        {
            const string sql =
                "SELECT * FROM i18n.get_localization_table(@0) WHERE COALESCE(\"key\", '') != '';";
            using (var db = DbProvider.Get(FrapidDbServer.GetMetaConnectionString(tenant), tenant).GetDatabase())
            {
                return db.Query<LocalizedResource>(sql, language);
            }
        }
    }
}