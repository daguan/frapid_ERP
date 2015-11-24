using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using Frapid.Framework;
using Frapid.Framework.Extensions;

namespace Frapid.ApplicationState.Cache
{
    public static class AppUsers
    {
        [Obsolete]
        public static async Task SetCurrentLoginAsync()
        {
            long globalLoginId = HttpContext.Current.User.Identity.Name.To<long>();
            await SetCurrentLoginAsync(globalLoginId);
        }

        [Obsolete]
        public static async Task SetCurrentLoginAsync(long globalLoginId)
        {
            if (globalLoginId > 0)
            {
                string key = globalLoginId.ToString(CultureInfo.InvariantCulture);

                if (MemoryCache.Default[key] == null)
                {
                    MetaLogin metaLogin = await MetaLogin.GetAsync(globalLoginId);
                    Dictionary<string, object> dictionary = GetDictionary(metaLogin);

                    CacheFactory.AddToDefaultCache("Dictionary" + key, dictionary);
                    CacheFactory.AddToDefaultCache(key, metaLogin);
                }
            }
        }

        [Obsolete]
        public static MetaLogin GetCurrent()
        {
            long globalLoginId = 0;

            if (HttpContext.Current.User != null)
            {
                long.TryParse(HttpContext.Current.User.Identity.Name, out globalLoginId);
            }

            return GetCurrent(globalLoginId);
        }

        [Obsolete]
        public static MetaLogin GetCurrent(long globalLoginId)
        {
            MetaLogin login = new MetaLogin();


            if (globalLoginId != 0)
            {
                login =
                    CacheFactory.GetFromDefaultCacheByKey(globalLoginId.ToString(CultureInfo.InvariantCulture)) as
                        MetaLogin;
            }

            return login ?? (new MetaLogin());
        }

        [Obsolete]
        private static Dictionary<string, object> GetDictionary(MetaLogin metaLogin)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if (metaLogin == null)
            {
                return dictionary;
            }

            dictionary.Add("Catalog", metaLogin.Catalog);
            dictionary.Add("GlobalLoginId", metaLogin.GlobalLoginId);
            dictionary.Add("LoginId", metaLogin.LoginId);

            return dictionary;
        }

        [Obsolete]
        public static string GetCurrentUserDb()
        {
            string catalog = GetCurrent().Catalog;
            return catalog;
        }

        public static string GetCatalog()
        {
            return "frapid";
        }

    }
}