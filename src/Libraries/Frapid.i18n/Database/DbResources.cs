using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.DataAccess;
using Frapid.i18n.Models;

namespace Frapid.i18n.Database
{
    public static class DbResources
    {
        public static async Task<Dictionary<string, string>> GetLocalizedResourcesAsync()
        {
            const string sql = "SELECT * FROM localization.localized_resource_view;";
            IEnumerable<dynamic> dbResources = await Factory.GetAsync<dynamic>(Factory.MetaDatabase, sql);
            Dictionary<string, string> resources = new Dictionary<string, string>();


            foreach (dynamic resource in dbResources)
            {
                string key = resource.Key;
                string value = resource.Value;

                resources.Add(key, value);
            }

            return resources;
        }

        public static async Task<IEnumerable<LocalizedResource>> GetLocalizationTableAsync(string language)
        {
            const string sql =
                "SELECT * FROM localization.get_localization_table(@Language) WHERE COALESCE(key, '') != '';";
            return await Factory.GetAsync<LocalizedResource>(Factory.MetaDatabase, sql);
        }
    }
}