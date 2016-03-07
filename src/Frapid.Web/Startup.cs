using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Frapid.Web
{
    public class Startup
    {       
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);


                var configuration = new HubConfiguration
                {
                    EnableJavaScriptProxies = true
                };

                map.RunSignalR(configuration);
                var authorizer = new HubAuthorizeAttribute();
                var module = new AuthorizeModule(null, authorizer);
                GlobalHost.HubPipeline.AddModule(module);
            });

            LogManager.InternalizeLogger();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NPocoConfig.Register();
            StartupRegistration.Register();
            EodTaskRegistration.Register();
            AccountConfig.Register(app);
        }
    }
}