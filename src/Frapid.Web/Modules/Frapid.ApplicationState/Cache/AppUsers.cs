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
        public static void SetCurrentLogin()
        {
            long globalLoginId = HttpContext.Current.User.Identity.Name.To<long>();
            SetCurrentLogin(globalLoginId);
        }

        public static void SetCurrentLogin(long globalLoginId)
        {
            if (globalLoginId <= 0)
            {
                return;
            }

            string key = globalLoginId.ToString(CultureInfo.InvariantCulture);

            if (MemoryCache.Default[key] != null)
            {
                return;
            }

            MetaLogin metaLogin = GetMetaLogin(globalLoginId);
            Dictionary<string, object> dictionary = GetDictionary(metaLogin);

            CacheFactory.AddToDefaultCache("Dictionary" + key, dictionary);
            CacheFactory.AddToDefaultCache(key, metaLogin);
        }

        public static MetaLogin GetCurrent()
        {
            long globalLoginId = 0;

            if (HttpContext.Current.User != null)
            {
                long.TryParse(HttpContext.Current.User.Identity.Name, out globalLoginId);
            }

            return GetCurrent(globalLoginId);
        }

        public static MetaLogin GetCurrent(long globalLoginId)
        {
            MetaLogin login = new MetaLogin();


            if (globalLoginId != 0)
            {
                var cacheObject = CacheFactory.GetFromDefaultCacheByKey(globalLoginId.ToString(CultureInfo.InvariantCulture));
                login = cacheObject as MetaLogin;
            }

            if (login == null)
            {
                login = new MetaLogin();
            }

            if (login.View == null)
            {
                login.View = new LoginView();
            }

            return login;
        }

        public static long GetMetaLoginId(string catalog, long loginId)
        {
            const string sql = "INSERT INTO public.frapid_logins(catalog, login_id) SELECT @0::text, @1::bigint RETURNING global_login_id;";
            return Factory.Scalar<long>(Factory.MetaDatabase, sql, catalog, loginId);
        }

        public static MetaLogin GetMetaLogin(long globalLoginId)
        {
            string sql = "SELECT * FROM public.frapid_logins WHERE global_login_id=@0;";
            MetaLogin login = Factory.Get<MetaLogin>(Factory.MetaDatabase, sql, globalLoginId).FirstOrDefault();

            if (login == null)
            {
                return null;
            }

            string catalog = login.Catalog;

            sql = "SELECT * FROM account.sign_in_view WHERE login_id=@0;";

            var view = Factory.Get<LoginView>(catalog, sql, login.LoginId).FirstOrDefault();

            if (view == null)
            {
                return null;
            }

            login.View = view;
            return login;
        }

        private static Dictionary<string, object> GetDictionary(MetaLogin metaLogin)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if (metaLogin == null)
            {
                return dictionary;
            }

            dictionary.Add("Catalog", metaLogin.Catalog);
            dictionary.Add("Culture", metaLogin.View.Culture);
            dictionary.Add("Email", metaLogin.View.Email);
            dictionary.Add("Office", metaLogin.View.Office);
            dictionary.Add("OfficeId", metaLogin.View.OfficeId);
            dictionary.Add("OfficeName", metaLogin.View.OfficeName);
            dictionary.Add("RoleName", metaLogin.View.RoleName);
            dictionary.Add("UserId", metaLogin.View.UserId);
            dictionary.Add("UserName", metaLogin.View.Email);

            return dictionary;
        }

        public static string GetCatalog()
        {
            return DbConvention.GetCatalog();
        }
    }
}