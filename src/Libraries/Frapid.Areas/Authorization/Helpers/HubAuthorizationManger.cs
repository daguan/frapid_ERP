using System.Threading.Tasks;
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
        public static async Task<long> GetLoginIdAsync(HubCallerContext context)
        {
            var token = await GetTokenAsync(context);

            if(token == null)
            {
                return 0;
            }

            return token.LoginId;
        }

        private static async Task<Token> GetTokenAsync(HubCallerContext context)
        {
            string clientToken = context.Request.GetClientToken();
            var provider = new Provider(TenantConvention.GetTenant());
            var token = provider.GetToken(clientToken);
            if(token != null)
            {
                bool isValid = await AccessTokens.IsValidAsync(token.ClientToken, context.Request.GetClientIpAddress(), context.Headers["User-Agent"]);

                if(isValid)
                {
                    return token;
                }
            }

            return null;
        }

        public static async Task<MetaUser> GetUserAsync(HubCallerContext context)
        {
            var token = await GetTokenAsync(context);

            if(token != null)
            {
                string tenant = TenantConvention.GetTenant();

                await AppUsers.SetCurrentLoginAsync(tenant, token.LoginId);
                var loginView = await AppUsers.GetCurrentAsync(tenant, token.LoginId);

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