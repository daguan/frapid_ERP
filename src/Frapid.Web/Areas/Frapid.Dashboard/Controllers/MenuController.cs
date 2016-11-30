using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.DAL;
using Frapid.i18n;

namespace Frapid.Dashboard.Controllers
{
    public class MenuController : FrapidController
    {
        [Route("dashboard/my/menus")]
        [RestrictAnonymous]
        public ActionResult GetMenus()
        {
            int userId = this.AppUser.UserId;
            int officeId = this.AppUser.OfficeId;
            string culture = CultureManager.GetCurrent().TwoLetterISOLanguageName;

            return this.Ok(Menus.GetAsync(this.Tenant, userId, officeId, culture));
        }
    }
}