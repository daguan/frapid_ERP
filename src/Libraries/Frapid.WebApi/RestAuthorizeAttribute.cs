using System;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Frapid.Framework.Extensions;
using Frapid.TokenManager.DAL;

namespace Frapid.WebApi
{
    public class RestAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = context.RequestContext.Principal as ClaimsPrincipal;

            if (user?.Identity == null)
            {
                return false;
            }

            long loginId = context.RequestContext.ReadClaim<long>("loginid");
            var expriesOn = new DateTime(context.RequestContext.ReadClaim<long>("exp"), DateTimeKind.Utc);
            string ipAddress = context.Request.GetClientIpAddress();
            string userAgent = context.Request.GetUserAgent();
            string clientToken = context.Request.GetBearerToken();


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
    }
}