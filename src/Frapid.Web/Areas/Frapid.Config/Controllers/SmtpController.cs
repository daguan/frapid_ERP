using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard;
using Frapid.Dashboard.Controllers;

namespace Frapid.Config.Controllers
{
    public class SmtpController : DashboardController
    {
        [Route("dashboard/config/smtp")]
        [RestrictAnonymous]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Smtp/Index.cshtml", this.Tenant));
        }
    }
}