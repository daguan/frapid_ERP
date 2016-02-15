using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.i18n;
using Frapid.TokenManager;
using Frapid.TokenManager.DAL;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

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
            string catalog = DbConvention.GetCatalog();

            if (token != null)
            {
                bool isValid = AccessTokens.IsValid(token.ClientToken, context.HttpContext.GetClientIpAddress(),
                    context.HttpContext.GetUserAgent());

                if (isValid)
                {
                    AppUsers.SetCurrentLogin(catalog, token.LoginId);
                    var loginView = AppUsers.GetCurrent(catalog, token.LoginId);

                    this.MetaUser = new MetaUser
                    {
                        Catalog = catalog,
                        ClientToken = token.ClientToken,
                        LoginId = token.LoginId,
                        UserId = token.UserId,
                        OfficeId = token.OfficeId
                    };

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

                    context.HttpContext.User = new ClaimsPrincipal(identity);
                }
            }

            base.Initialize(context);
        }

        protected ActionResult Ok(object model = null)
        {
            if (model == null)
            {
                model = "OK";
            }

            string json = JsonConvert.SerializeObject(model);
            return this.Content(json, "application/json");
        }

        protected ActionResult Failed(string message, HttpStatusCode statusCode)
        {
            this.Response.StatusCode = (int)statusCode;
            return this.Content(message, MediaTypeNames.Text.Plain, Encoding.UTF8);
        }
    }
}