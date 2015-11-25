using System.Collections.Generic;
using Frapid.DataAccess;
using Frapid.i18n.Models;

namespace Frapid.i18n.Database
{
    public static class DbResources
    {
        public static Dictionary<string, string> GetLocalizedResources()
        {
            const string sql = "SELECT * FROM i18n.localized_resource_view;";
            IEnumerable<dynamic> dbResources = Factory.Get<dynamic>(Factory.MetaDatabase, sql);
            Dictionary<string, string> resources = new Dictionary<string, string>();


            foreach (dynamic resource in dbResources)
            {
                string key = resource.Key;
                string value = resource.Value;

                resources.Add(key, value);
            }

            return resources;
        }

        public static IEnumerable<LocalizedResource> GetLocalizationTable(string language)
        {
            const string sql =
                "SELECT * FROM i18n.get_localization_table(@0) WHERE COALESCE(key, '') != '';";
            return Factory.Get<LocalizedResource>(Factory.MetaDatabase, sql, language);
        }
    }
}