using System.Collections.Generic;
using Frapid.Configuration;
using Frapid.i18n.Models;

namespace Frapid.i18n.DAL
{
    public static class DbResources
    {
        static readonly string MetaCatalog = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");

        public static Dictionary<string, string> GetLocalizedResources()
        {
            const string sql = "SELECT * FROM i18n.localized_resource_view;";

            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(MetaCatalog)).GetDatabase())
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

        public static IEnumerable<LocalizedResource> GetLocalizationTable(string language)
        {
            const string sql =
                "SELECT * FROM i18n.get_localization_table(@0) WHERE COALESCE(key, '') != '';";
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(MetaCatalog)).GetDatabase())
            {
                return db.Query<LocalizedResource>(sql, language);
            }
        }
    }
}