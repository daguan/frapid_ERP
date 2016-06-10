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
using Serilog;

namespace Frapid.WebApi
{
    public class FrapidApiController: ApiController
    {
        public string Tenant { get; set; }
        public MetaUser MetaUser { get; set; }

        protected override async void Initialize(HttpControllerContext context)
        {
            this.Tenant = TenantConvention.GetTenant();

            string clientToken = context.Request.GetBearerToken();
            var provider = new Provider(this.Tenant);
            var token = provider.GetToken(clientToken);


            if(token != null)
            {
                await AppUsers.SetCurrentLoginAsync(this.Tenant, token.LoginId).ConfigureAwait(false);
                var loginView = await AppUsers.GetCurrentAsync(this.Tenant, token.LoginId).ConfigureAwait(false);

                this.MetaUser = new MetaUser
                                {
                                    Tenant = this.Tenant,
                                    ClientToken = token.ClientToken,
                                    LoginId = token.LoginId,
                                    UserId = loginView.UserId.To<int>(),
                                    OfficeId = loginView.OfficeId.To<int>()
                                };

                var identity = new ClaimsIdentity(token.Claims);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, token.LoginId.ToString(CultureInfo.InvariantCulture)));

                if(loginView.RoleName != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, loginView.RoleName));
                }

                if(loginView.Email != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Email, loginView.Email));
                }

                context.RequestContext.Principal = new ClaimsPrincipal(identity);
            }

            base.Initialize(context);
        }

        public static List<Assembly> GetMembers()
        {
            var type = typeof(FrapidApiController);
            try
            {
                var items = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => p.IsSubclassOf(type)).Select(t => t.Assembly).ToList();
                return items;
            }
            catch(ReflectionTypeLoadException ex)
            {
                Log.Error("Could not register API members. {Exception}", ex);
                //Swallow the exception
            }

            return null;
        }
    }
}