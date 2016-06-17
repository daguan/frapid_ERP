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
        public AppUser AppUser { get; set; }
        public string CurrentDomain { get; set; }
        public string Tenant { get; set; }
        public string CurrentPageUrl { get; set; }

        protected new virtual ContentResult View(string viewName, object model = null)
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

            string html = CdnHelper.UseCdn(output.ToString());
            html = MinificationHelper.Minify(html);

            return this.Content(html, "text/html");
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            if (this.Request?.Url != null)
            {
                this.CurrentDomain = this.Request.Url.DnsSafeHost;
                this.CurrentPageUrl = this.Request.Url.AbsoluteUri;
                this.Tenant = TenantConvention.GetTenant(this.CurrentDomain);
            }

            this.RemoteUser = RemoteUser.Get(this.HttpContext);
            this.Initialize(context.RequestContext);
        }


        protected override void Initialize(RequestContext context)
        {
            string tenant = TenantConvention.GetTenant();
            string clientToken = context.HttpContext.Request.GetClientToken();
            var provider = new Provider();
            var token = provider.GetToken(clientToken);

            if (token != null)
            {
                bool isValid = AccessTokens.IsValidAsync(tenant, token.ClientToken, context.HttpContext.GetClientIpAddress(),
                            context.HttpContext.GetUserAgent()).Result;

                if (isValid)
                {
                    AppUsers.SetCurrentLoginAsync(tenant, token.LoginId).Wait();
                    var loginView = AppUsers.GetCurrentAsync(tenant, token.LoginId).Result;

                    this.AppUser = new AppUser
                    {
                        Tenant = tenant,
                        ClientToken = token.ClientToken,
                        LoginId = loginView.LoginId,
                        UserId = loginView.UserId,
                        Name = loginView.Name,
                        OfficeId = loginView.OfficeId,
                        OfficeName = loginView.OfficeName,
                        Email = loginView.Email,
                        RoleId = loginView.RoleId,
                        RoleName = loginView.RoleName,
                        IsAdministrator = loginView.IsAdministrator
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
            this.Response.StatusCode = (int)statusCode;
            return this.Content(message, MediaTypeNames.Text.Plain, Encoding.UTF8);
        }

        protected ActionResult InvalidModelState()
        {
            return this.Failed("Invalid model state", HttpStatusCode.BadRequest);
        }

        protected ActionResult AccessDenied()
        {
            return this.Failed("Access is denied", HttpStatusCode.Forbidden);
        }
    }
}