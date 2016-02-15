using System.Web.Mvc;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.Controllers;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.Controllers.Backend
{
    public class ContentController : DashboardController
    {
        [Route("dashboard/website/contents")]
        [RestrictAnonymous]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Index.cshtml"));
        }

        [Route("dashboard/website/contents/manage")]
        [Route("dashboard/website/contents/new")]
        [RestrictAnonymous]
        public ActionResult Manage(int contentId = 0)
        {
            var model = Contents.Get(contentId) ?? new Content();
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Content/Manage.cshtml"), model);
        }
    }
}