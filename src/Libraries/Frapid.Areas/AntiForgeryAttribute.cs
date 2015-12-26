using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Frapid.Areas
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AntiForgeryAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request.HttpMethod != WebRequestMethods.Http.Post)
            {
                return;
            }

            if (request.IsAjaxRequest())
            {
                var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                string cookieValue = antiForgeryCookie?.Value;
                AntiForgery.Validate(cookieValue, request.Headers["RequestVerificationToken"]);
            }
            else
            {
                new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
            }
        }
    }
}