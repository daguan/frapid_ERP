using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Frapid.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (Startup))]
namespace Frapid.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LogManager.InternalizeLogger();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NPocoConfig.Register();
            StartupRegistration.Register();
            AccountConfig.Register(app);
        }
    }
}