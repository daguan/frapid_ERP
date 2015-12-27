using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework;
using Frapid.Framework.Extensions;

namespace Frapid.ApplicationState.Cache
{
    public static class AppUsers
    {
        public static void SetCurrentLogin(string catalog)
        {
            long globalLoginId = HttpContext.Current.User.Identity.Name.To<long>();
            SetCurrentLogin(catalog, globalLoginId);
        }

        public static void SetCurrentLogin(string catalog, long loginId)
        {
            if (loginId <= 0)
            {
                return;
            }

            string key = catalog + "-" + loginId.ToString(CultureInfo.InvariantCulture);

            if (MemoryCache.Default[key] != null)
            {
                return;
            }

            var metaLogin = GetMetaLogin(catalog, loginId);
            var dictionary = GetDictionary(catalog, metaLogin);

            CacheFactory.AddToDefaultCache("Dictionary" + key, dictionary);
            CacheFactory.AddToDefaultCache(key, metaLogin);
        }

        public static LoginView GetCurrent(string catalog = "")
        {
            if (string.IsNullOrWhiteSpace(catalog))
            {
                catalog = GetCatalog();
            }

            long loginId = 0;

            if (HttpContext.Current.User != null)
            {
                long.TryParse(HttpContext.Current.User.Identity.Name, out loginId);
            }

            return GetCurrent(catalog, loginId);
        }

        public static LoginView GetCurrent(string catalog, long loginId)
        {
            var login = new LoginView();

            if (loginId == 0)
            {
                return login;
            }

            string key = catalog + "-" + loginId.ToString(CultureInfo.InvariantCulture);
            var cacheObject = CacheFactory.GetFromDefaultCacheByKey(key);
            login = cacheObject as LoginView;

            return login ?? new LoginView();
        }

        public static long GetMetaLoginId(string catalog, long loginId)
        {
            const string sql = "INSERT INTO public.frapid_logins(catalog, login_id) SELECT @0::text, @1::bigint RETURNING global_login_id;";
            return Factory.Scalar<long>(Factory.MetaDatabase, sql, catalog, loginId);
        }

        public static LoginView GetMetaLogin(string catalog, long loginId)
        {
            const string sql = "SELECT * FROM account.sign_in_view WHERE login_id=@0;";
            var view = Factory.Get<LoginView>(catalog, sql, loginId).FirstOrDefault();
            return view;
        }

        private static Dictionary<string, object> GetDictionary(string catalog, LoginView metaLogin)
        {
            var dictionary = new Dictionary<string, object>();

            if (metaLogin == null)
            {
                return dictionary;
            }

            dictionary.Add("Catalog", catalog);
            dictionary.Add("Culture", metaLogin.Culture);
            dictionary.Add("Email", metaLogin.Email);
            dictionary.Add("Office", metaLogin.Office);
            dictionary.Add("OfficeId", metaLogin.OfficeId);
            dictionary.Add("OfficeName", metaLogin.OfficeName);
            dictionary.Add("RoleName", metaLogin.RoleName);
            dictionary.Add("UserId", metaLogin.UserId);
            dictionary.Add("UserName", metaLogin.Email);

            return dictionary;
        }

        public static string GetCatalog()
        {
            return DbConvention.GetCatalog();
        }
    }
}