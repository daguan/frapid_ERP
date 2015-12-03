using System.Web.Mvc;
using System.Web.Routing;
using Frapid.ApplicationState.Cache;

namespace Frapid.Web
{
    public class MetaConfig
    {
        public static void Setup()
        {
            MetaLoginHelper.CreateTable();
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute("Dashboard", "dashboard/{controller}/{action}/{id}",
                new {controller = "Dashboard", action = "Index", id = UrlParameter.Optional});
        }
    }
}