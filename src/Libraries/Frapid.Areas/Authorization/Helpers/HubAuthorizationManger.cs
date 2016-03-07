using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.TokenManager;
using Frapid.TokenManager.DAL;
using Microsoft.AspNet.SignalR.Hubs;

namespace Frapid.Areas.Authorization.Helpers
{
    public static class HubAuthorizationManger
    {
        public static long GetLoginId(HubCallerContext context)
        {
            var token = GetToken(context);

            if (token == null)
            {
                return 0;
            }

            return token.LoginId;
        }

        private static Token GetToken(HubCallerContext context)
        {
            string clientToken = context.Request.GetClientToken();
            var provider = new Provider(DbConvention.GetTenant());
            var token = provider.GetToken(clientToken);
            if (token != null)
            {
                bool isValid = AccessTokens.IsValid(token.ClientToken, context.Request.GetClientIpAddress(),
                    context.Headers["User-Agent"]);

                if (isValid)
                {
                    return token;
                }
            }

            return null;
        }

        public static MetaUser GetUser(HubCallerContext context)
        {
            var token = GetToken(context);

            if (token != null)
            {
                string tenant = DbConvention.GetTenant();

                AppUsers.SetCurrentLogin(tenant, token.LoginId);
                var loginView = AppUsers.GetCurrent(tenant, token.LoginId);

                return new MetaUser
                {
                    Tenant = tenant,
                    ClientToken = token.ClientToken,
                    LoginId = token.LoginId,
                    UserId = loginView.UserId,
                    OfficeId = loginView.OfficeId
                };

            }

            return null;
        }
    }
}