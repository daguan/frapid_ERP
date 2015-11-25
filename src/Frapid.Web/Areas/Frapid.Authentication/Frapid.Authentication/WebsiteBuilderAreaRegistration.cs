using System.Web.Mvc;

namespace Frapid.Authentication
{
    public class AuthenticationAreaRegistration: AreaRegistration
    {
        public override string AreaName => "Authentication";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.LowercaseUrls = true;
            context.Routes.MapMvcAttributeRoutes();
        }
    }
}