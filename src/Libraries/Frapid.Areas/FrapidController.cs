using System.Globalization;
using System.Security.Claims;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.i18n;
using Frapid.TokenManager;
using Frapid.TokenManager.DAL;
using Microsoft.AspNet.Identity;

namespace Frapid.Areas
{
    public abstract class FrapidController : Controller
    {
        public RemoteUser RemoteUser { get; private set; }
        public MetaUser MetaUser { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            this.RemoteUser = new RemoteUser
            {
                Browser = this.Request?.Browser.Browser,
                IpAddress = this.Request?.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name,
                UserAgent = this.Request?.UserAgent
            };
        }

        protected override void Initialize(RequestContext context)
        {
            string clientToken = context.HttpContext.Request.GetClientToken();
            var provider = new Provider(DbConvention.GetCatalog());
            var token = provider.GetToken(clientToken);

            if (token != null)
            {
                bool isValid = AccessTokens.IsValid(token.ClientToken, context.HttpContext.GetClientIpAddress(), context.HttpContext.GetUserAgent());

                if (isValid)
                {
                    AppUsers.SetCurrentLogin(token.LoginId);

                    this.MetaUser = new MetaUser
                    {
                        Catalog = DbConvention.GetCatalog(),
                        ClientToken = token.ClientToken,
                        LoginId = token.LoginId,
                        UserId = token.UserId,
                        OfficeId = token.OfficeId
                    };

                    var identity = new ClaimsIdentity(token.Claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);

                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token.LoginId.ToString(CultureInfo.InvariantCulture)));
                    identity.AddClaim(new Claim(ClaimTypes.Role, AppUsers.GetCurrent(token.LoginId).View.RoleName));
                    identity.AddClaim(new Claim(ClaimTypes.Email, AppUsers.GetCurrent(token.LoginId).View.Email));

                    context.HttpContext.User = new ClaimsPrincipal(identity);
                }
            }

            base.Initialize(context);
        }

        protected string GetRazorView(string areaName, string path)
        {
            string catalog = DbConvention.GetCatalog();

            string overridePath = "~/Catalogs/{0}/Areas/{1}/Views/" + path;
            overridePath = string.Format(CultureInfo.InvariantCulture, overridePath, catalog, areaName);

            if (System.IO.File.Exists(HostingEnvironment.MapPath(overridePath)))
            {
                return overridePath;
            }

            string defaultPath = "~/Areas/{0}/Views/{1}";
            defaultPath = string.Format(CultureInfo.InvariantCulture, defaultPath, areaName, path);

            return defaultPath;
        }

        protected string GetRazorView(string areaName, string controllerName, string actionName)
        {
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(areaName, path);
        }

        protected string GetRazorView<T>(string path) where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            return this.GetRazorView(registration.AreaName, path);
        }

        protected string GetRazorView<T>(string controllerName, string actionName)
            where T : FrapidAreaRegistration, new()
        {
            FrapidAreaRegistration registration = new T();
            string path = controllerName.ToLower() + "/" + actionName.ToLower() + ".cshtml";
            return this.GetRazorView(registration.AreaName, path);
        }
    }
}