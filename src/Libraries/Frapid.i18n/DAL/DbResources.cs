using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.i18n.Models;
using Frapid.Mapper.Query.Select;

namespace Frapid.i18n.DAL
{
    public static class DbResources
    {
        public static async Task<Dictionary<string, string>> GetLocalizedResourcesAsync(string tenant)
        {
            const string sql = "SELECT * FROM i18n.localized_resource_view;";

            using(var db = DbProvider.Get(FrapidDbServer.GetMetaConnectionString(tenant), tenant).GetDatabase())
            {
                var dbResources = await db.SelectAsync<dynamic>(sql).ConfigureAwait(false);

                var resources = new Dictionary<string, string>();

                foreach(var resource in dbResources)
                {
                    string key = resource.Key;
                    string value = resource.Value;

                    resources.Add(key, value);
                }

                return resources;
            }
        }

        public static async Task<IEnumerable<LocalizedResource>> GetLocalizationTableAsync(string tenant, string language)
        {
            const string sql = "SELECT * FROM i18n.get_localization_table(@0) WHERE COALESCE(\"key\", '') != '';";
            using(var db = DbProvider.Get(FrapidDbServer.GetMetaConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.SelectAsync<LocalizedResource>(sql, language).ConfigureAwait(false);
            }
        }
    }
}