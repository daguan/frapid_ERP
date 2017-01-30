using System.Web.Mvc;

namespace Frapid.Calendar.Controllers.Backend
{
    public sealed class CalendarController : CalendarBackendController
    {
        [Route("dashboard/calendar")]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Backend/Index.cshtml", this.Tenant));
        }
    }
}