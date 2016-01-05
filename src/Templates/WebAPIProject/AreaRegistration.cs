using System.Web.Mvc;
using Frapid.Areas;

namespace WebAPIProject
{
    public class AreaRegistration : FrapidAreaRegistration
    {
        public override string AreaName => "WebAPIProject";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.LowercaseUrls = true;
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}