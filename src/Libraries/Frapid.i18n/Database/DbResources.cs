using System.Collections.Generic;
using Frapid.Configuration;
using Frapid.i18n.Models;

namespace Frapid.i18n.Database
{
    public static class DbResources
    {
        static readonly string MetaCatalog = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "MetaDatabase");

        public static Dictionary<string, string> GetLocalizedResources()
        {
            const string sql = "SELECT * FROM i18n.localized_resource_view;";

            using (NPoco.Database db = Provider.Get(ConnectionString.GetConnectionString(MetaCatalog)).GetDatabase())
            {
                IEnumerable<dynamic> dbResources = db.Query<dynamic>(sql);

                Dictionary<string, string> resources = new Dictionary<string, string>();


                foreach (dynamic resource in dbResources)
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
            using (NPoco.Database db = Provider.Get(ConnectionString.GetConnectionString(MetaCatalog)).GetDatabase())
            {
                return db.Query<LocalizedResource>(sql, language);
            }
        }
    }
}