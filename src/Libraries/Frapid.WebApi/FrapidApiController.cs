using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.TokenManager;

namespace Frapid.WebApi
{
    public class FrapidApiController : ApiController
    {
        public MetaUser MetaUser { get; set; }

        protected override void Initialize(HttpControllerContext context)
        {
            string clientToken = context.Request.GetBearerToken();
            var provider = new Provider(DbConvention.GetCatalog());
            var token = provider.GetToken(clientToken);

            if (token != null)
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

                var identity = new ClaimsIdentity(token.Claims);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token.LoginId.ToString(CultureInfo.InvariantCulture)));
                identity.AddClaim(new Claim(ClaimTypes.Role, AppUsers.GetCurrent(token.LoginId).View.RoleName));
                identity.AddClaim(new Claim(ClaimTypes.Email, AppUsers.GetCurrent(token.LoginId).View.Email));

                context.RequestContext.Principal = new ClaimsPrincipal(identity);
            }

            base.Initialize(context);
        }

        public static List<Assembly> GetMembers()
        {
            var type = typeof (FrapidApiController);
            try
            {
                var items = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.IsSubclassOf(type)).Select(t => t.Assembly).ToList();
                return items;
            }
            catch (ReflectionTypeLoadException)
            {
                //Swallow the exception
            }

            return null;
        }
    }
}