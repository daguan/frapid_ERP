using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Frapid.ApplicationState.CacheFactory;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
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

            var factory = new DefaultCacheFactory();

            var login = factory.Get<LoginView>(key);
            if (login != null)
            {
                return login;
            }

            login = GetMetaLogin(tenant, loginId);
            var dictionary = GetDictionary(tenant, login);


            factory.Add(tenant + "/dictionary/" + key, dictionary, DateTimeOffset.UtcNow.AddHours(2));
            factory.Add(key, login, DateTimeOffset.UtcNow.AddHours(2));

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

            var factory = new DefaultCacheFactory();

            login = factory.Get<LoginView>(key);

            var view = login ?? SetCurrentLogin(tenant, loginId);

            UpdateActivity(view.UserId.To<int>(), view.IpAddress, view.Browser);
            return view;
        }

        private static void UpdateActivity(int userId, string ip, string browser)
        {
            DAL.AppUsers.UpdateActivity(GetTenant(), userId, ip, browser);
        }

        public static LoginView GetMetaLogin(string database, long loginId)
        {
            return DAL.AppUsers.GetMetaLogin(database, loginId);
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