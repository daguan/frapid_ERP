using System.Globalization;
using System.Security.Claims;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.TokenManager;
using Frapid.TokenManager.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;

namespace Frapid.Areas.Authorization
{
    public class HubAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor descriptor, IRequest request)
        {
            string clientToken = request.GetClientToken();
            var provider = new Provider(DbConvention.GetCatalog());
            var token = provider.GetToken(clientToken);
            string catalog = DbConvention.GetCatalog();

            if (token != null)
            {
                bool isValid = AccessTokens.IsValid(token.ClientToken, request.GetClientIpAddress(),
                    request.Headers["user-agent"]);

                if (isValid)
                {
                    AppUsers.SetCurrentLogin(catalog, token.LoginId);
                    var loginView = AppUsers.GetCurrent(catalog, token.LoginId);

                    var identity = new ClaimsIdentity(token.Claims, DefaultAuthenticationTypes.ApplicationCookie,
                        ClaimTypes.NameIdentifier, ClaimTypes.Role);

                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                        token.LoginId.ToString(CultureInfo.InvariantCulture)));

                    if (loginView.RoleName != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, loginView.RoleName));
                    }

                    if (loginView.Email != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Email, loginView.Email));
                    }

                    request.Environment["server.User"] = new ClaimsPrincipal(identity);
                    return true;
                }
            }

            return false;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext invoker,
            bool appliesToMethod)
        {
            string connectionId = invoker.Hub.Context.ConnectionId;
            var environment = invoker.Hub.Context.Request.Environment;
            var principal = environment["server.User"] as ClaimsPrincipal;

            if (principal?.Identity != null && principal.Identity.IsAuthenticated)
            {
                // create a new HubCallerContext instance with the principal generated from token
                // and replace the current context so that in hubs we can retrieve current user identity
                invoker.Hub.Context = new HubCallerContext(new ServerRequest(environment), connectionId);

                return true;
            }

            return false;
        }
    }
}