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
        public static void SetCurrentLogin(string tenant)
        {
            long globalLoginId = HttpContext.Current.User.Identity.Name.To<long>();
            SetCurrentLogin(tenant, globalLoginId);
        }

        public static LoginView SetCurrentLogin(string tenant, long loginId)
        {
            if (loginId <= 0)
            {
                return new LoginView();
            }

            string key = tenant + "-" + loginId.ToString(CultureInfo.InvariantCulture);

            var cacheObject = CacheFactory.GetFromDefaultCacheByKey(key);
            var login = cacheObject as LoginView;

            if (login != null)
            {
                return login;
            }

            login = GetMetaLogin(tenant, loginId);
            var dictionary = GetDictionary(tenant, login);

            CacheFactory.AddToDefaultCache("Dictionary" + key, dictionary);
            CacheFactory.AddToDefaultCache(key, login);

            return login;
        }

        public static LoginView GetCurrent(string tenant = "")
        {
            if (string.IsNullOrWhiteSpace(tenant))
            {
                tenant = GetTenant();
            }

            long loginId = 0;

            if (HttpContext.Current.User != null)
            {
                long.TryParse(HttpContext.Current.User.Identity.Name, out loginId);
            }

            return GetCurrent(tenant, loginId);
        }

        public static LoginView GetCurrent(string tenant, long loginId)
        {
            var login = new LoginView();

            if (loginId == 0)
            {
                return login;
            }

            string key = tenant + "-" + loginId.ToString(CultureInfo.InvariantCulture);
            var cacheObject = CacheFactory.GetFromDefaultCacheByKey(key);
            login = cacheObject as LoginView;

            var view = login ?? SetCurrentLogin(tenant, loginId);

            UpdateActivity(view.UserId.To<int>(), view.IpAddress, view.Browser);
            return view;
        }

        private static void UpdateActivity(int userId, string ip, string browser)
        {
            const string sql = "UPDATE account.users SET last_seen_on=NOW(), last_ip=@0, last_browser=@1 WHERE user_id=@2;";
            Factory.NonQuery(GetTenant(), sql, ip, browser, userId);
        }

        public static long GetMetaLoginId(string database, long loginId)
        {
            const string sql = "INSERT INTO public.frapid_logins(tenant, login_id) SELECT @0::text, @1::bigint RETURNING global_login_id;";
            return Factory.Scalar<long>(Factory.MetaDatabase, sql, database, loginId);
        }

        public static LoginView GetMetaLogin(string database, long loginId)
        {
            const string sql = "SELECT * FROM account.sign_in_view WHERE login_id=@0;";
            var view = Factory.Get<LoginView>(database, sql, loginId).FirstOrDefault();
            return view;
        }

        private static Dictionary<string, object> GetDictionary(string database, LoginView metaLogin)
        {
            var dictionary = new Dictionary<string, object>();

            if (metaLogin == null)
            {
                return dictionary;
            }

            dictionary.Add("Database", database);
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

        public static string GetTenant()
        {
            return DbConvention.GetTenant();
        }
    }
}