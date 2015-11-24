using System.Web.Mvc;

namespace WebsiteBuilder
{
    public class WebsiteBuilderAreaRegistration: AreaRegistration
    {
        public override string AreaName => "WebsiteBuilder";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.LowercaseUrls = true;
            context.Routes.MapMvcAttributeRoutes();

            context.MapRoute("Homepage", "{controller}/{action}", new { action = "Index", id = UrlParameter.Optional }, new[] { "WebsiteBuilder.Controllers" });
            context.MapRoute("Pages", "site/{*.}", new { action = "Index", id = UrlParameter.Optional }, new[] { "WebsiteBuilder.Controllers" });
        }
    }
}