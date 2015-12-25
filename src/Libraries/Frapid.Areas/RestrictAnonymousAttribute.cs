using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Frapid.Framework.Extensions;
using Frapid.TokenManager.DAL;

namespace Frapid.Areas
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RestrictAnonymousAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.RequestContext.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                context.Result = new RedirectResult("/account/sign-in");
            }
        }

        private bool IsAuthorized(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }


            var user = context.User as ClaimsPrincipal;

            if (user?.Identity == null)
            {
                return false;
            }

            long loginId = context.ReadClaim<long>("loginid");
            var expriesOn = new DateTime(context.ReadClaim<long>("exp"), DateTimeKind.Utc);
            string ipAddress = context.GetClientIpAddress();
            string userAgent = context.GetUserAgent();
            string clientToken = context.Request.GetClientToken();


            if (string.IsNullOrWhiteSpace(clientToken))
            {
                return false;
            }

            if (expriesOn <= DateTime.UtcNow)
            {
                return false;
            }

            return loginId > 0 && AccessTokens.IsValid(clientToken, ipAddress, userAgent);
        }

        protected override bool AuthorizeCore(HttpContextBase context)
        {
            bool authorized = this.IsAuthorized(context);

            if (!authorized)
            {
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            }

            return authorized;
        }
    }
}