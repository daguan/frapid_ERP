using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Frapid.Framework.Extensions;
using Frapid.TokenManager.DAL;
using Serilog;

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
            long userId = context.ReadClaim<int>("userid");
            long officeId = context.ReadClaim<int>("officeid");
            string email = context.ReadClaim<string>(ClaimTypes.Email);

            var expriesOn = new DateTime(context.ReadClaim<long>("exp"), DateTimeKind.Utc);
            string ipAddress = context.GetClientIpAddress();
            string userAgent = context.GetUserAgent();
            string clientToken = context.Request.GetClientToken();


            if (string.IsNullOrWhiteSpace(clientToken))
            {
                return false;
            }

            if (loginId <= 0)
            {
                Log.Warning(
                    "Invalid login claims supplied. Access was denied to user {userId}/{email} for officeId {officeId} having the loginId {loginId}. Token: {clientToken}.",
                    userId, email, officeId, loginId, clientToken);
                Thread.Sleep(new Random().Next(1, 60)*1000);
                return false;
            }

            if (expriesOn <= DateTime.UtcNow)
            {
                Log.Debug(
                    "Token expired. Access was denied to user {userId}/{email} for officeId {officeId} having the loginId {loginId}. Token: {clientToken}.",
                    userId, email, officeId, loginId, clientToken);
                return false;
            }


            bool isValid = AccessTokens.IsValid(clientToken, ipAddress, userAgent);

            if (expriesOn <= DateTime.UtcNow)
            {
                Log.Debug(
                    "Token invalid. Access was denied to user {userId}/{email} for officeId {officeId} having the loginId {loginId}. Token: {clientToken}.",
                    userId, email, officeId, loginId, clientToken);
                return false;
            }

            return isValid;
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