using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using Frapid.ApplicationState.CacheFactory;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
using Frapid.Framework;
using Frapid.Framework.Extensions;

namespace Frapid.ApplicationState.Cache
{
    public static class AppUsers
    {
        public static async Task SetCurrentLoginAsync(HttpContext context, string tenant)
        {
            long globalLoginId = context.User.Identity.Name.To<long>();
            await SetCurrentLoginAsync(tenant, globalLoginId);
        }

        public static async Task<LoginView> SetCurrentLoginAsync(string tenant, long loginId)
        {
            if(loginId <= 0)
            {
                return new LoginView();
            }

            string key = tenant + "-" + loginId.ToString(CultureInfo.InvariantCulture);

            var factory = new DefaultCacheFactory();

            var login = factory.Get<Task<LoginView>>(key);
            if(login != null)
            {
                return await login;
            }

            login = GetMetaLoginAsync(tenant, loginId);
            var awaiter = await login;

            var dictionary = GetDictionary(tenant, awaiter);


            factory.Add(tenant + "/dictionary/" + key, dictionary, DateTimeOffset.UtcNow.AddHours(2));
            factory.Add(key, login, DateTimeOffset.UtcNow.AddHours(2));

            return awaiter;
        }

        public static LoginView GetCurrent(string tenant = "")
        {
            return GetCurrentAsync(tenant).Result;
        }

        public static async Task<LoginView> GetCurrentAsync(string tenant = "")
        {
            var context = FrapidHttpContext.GetCurrent();
            if(string.IsNullOrWhiteSpace(tenant))
            {
                tenant = GetTenant();
            }

            long loginId = 0;

            if(context.User != null)
            {
                long.TryParse(context.User.Identity.Name, out loginId);
            }

            return await GetCurrentAsync(tenant, loginId);
        }

        public static async Task<LoginView> GetCurrentAsync(string tenant, long loginId)
        {
            if(loginId == 0)
            {
                return new LoginView();
            }

            string key = tenant + "-" + loginId.ToString(CultureInfo.InvariantCulture);

            var factory = new DefaultCacheFactory();

            var login = factory.Get<Task<LoginView>>(key);

            var view = login ?? SetCurrentLoginAsync(tenant, loginId);
            var awaiter = await view;

            await UpdateActivityAsync(awaiter.UserId.To<int>(), awaiter.IpAddress, awaiter.Browser);

            return await view;
        }

        private static async Task UpdateActivityAsync(int userId, string ip, string browser)
        {
            await DAL.AppUsers.UpdateActivityAsync(GetTenant(), userId, ip, browser);
        }

        public static async Task<LoginView> GetMetaLoginAsync(string database, long loginId)
        {
            return await DAL.AppUsers.GetMetaLoginAsync(database, loginId);
        }

        private static Dictionary<string, object> GetDictionary(string database, LoginView metaLogin)
        {
            var dictionary = new Dictionary<string, object>();

            if(metaLogin == null)
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
            return TenantConvention.GetTenant();
        }
    }
}