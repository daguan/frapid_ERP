using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
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
        public async Task<ActionResult> GetMenusAsync()
        {
            var user = await AppUsers.GetCurrentAsync();

            int userId = user.UserId;
            int officeId = user.OfficeId;
            string culture = CultureManager.GetCurrent().TwoLetterISOLanguageName;
            string tenant = AppUsers.GetTenant();

            return this.Ok(Menu.GetAsync(tenant, userId, officeId, culture));
        }
    }
}