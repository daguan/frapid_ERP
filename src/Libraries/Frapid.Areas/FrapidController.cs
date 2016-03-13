using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
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
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace Frapid.Areas
{
    public abstract class FrapidController : Controller
    {
        public RemoteUser RemoteUser { get; private set; }
        public MetaUser MetaUser { get; set; }

        protected new virtual ContentResult View(string viewName, object model)
        {
            var controllerContext = this.ControllerContext;
            var result = ViewEngines.Engines.FindView(controllerContext, viewName, null);

            StringWriter output;
            using (output = new StringWriter())
            {
                var dictionary = new ViewDataDictionary(model);

                var dynamic = this.ViewBag as DynamicObject;

                if (dynamic != null)
                {
                    var members = dynamic.GetDynamicMemberNames().ToList();

                    foreach (string member in members)
                    {
                        var value = Versioned.CallByName(dynamic, member, CallType.Get);
                        dictionary.Add(member, value);
                    }
                }


                var viewContext = new ViewContext(controllerContext, result.View, dictionary,
                    controllerContext.Controller.TempData, output);
                result.View.Render(viewContext, output);
                result.ViewEngine.ReleaseView(controllerContext, result.View);
            }

            return this.Content(CdnHelper.UseCdn(output.ToString()), "text/html");
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            this.RemoteUser = new RemoteUser
            {
                Browser = this.Request?.Browser.Browser,
                IpAddress = this.Request?.UserHostAddress,
                Culture = CultureManager.GetCurrent().Name,
                UserAgent = this.Request?.UserAgent,
                Country = this.Request?.ServerVariables["HTTP_CF_IPCOUNTRY"]
            };
        }

        protected override void Initialize(RequestContext context)
        {
            string clientToken = context.HttpContext.Request.GetClientToken();
            var provider = new Provider(DbConvention.GetTenant());
            var token = provider.GetToken(clientToken);
            string tenant = DbConvention.GetTenant();

            if (token != null)
            {
                bool isValid = AccessTokens.IsValid(token.ClientToken, context.HttpContext.GetClientIpAddress(),
                    context.HttpContext.GetUserAgent());

                if (isValid)
                {
                    AppUsers.SetCurrentLogin(tenant, token.LoginId);
                    var loginView = AppUsers.GetCurrent(tenant, token.LoginId);

                    this.MetaUser = new MetaUser
                    {
                        Tenant = tenant,
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

            this.Response.StatusCode = 200;
            string json = JsonConvert.SerializeObject(model);
            return this.Content(json, "application/json");
        }

        protected ActionResult Failed(string message, HttpStatusCode statusCode)
        {
            this.Response.StatusCode = (int) statusCode;
            return this.Content(message, MediaTypeNames.Text.Plain, Encoding.UTF8);
        }
    }
}