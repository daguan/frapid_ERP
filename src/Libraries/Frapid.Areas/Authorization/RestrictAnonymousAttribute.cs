using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Frapid.Areas.Authorization.Helpers;
using Frapid.Framework;

namespace Frapid.Areas.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class RestrictAnonymousAttribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if(!context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new RedirectResult("/account/sign-in");
            }
        }

        protected override bool AuthorizeCore(HttpContextBase context)
        {
            bool authorized = AuthorizationManager.IsAuthorizedAsync(context).Result;

            if(!authorized)
            {
                FrapidHttpContext.GetCurrent().User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            }

            return authorized;
        }
    }
}