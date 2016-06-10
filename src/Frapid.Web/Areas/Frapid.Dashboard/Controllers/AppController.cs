using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Areas.Authorization;
using Frapid.Dashboard.DAL;
using Frapid.i18n;

namespace Frapid.Dashboard.Controllers
{
    public class AppController: FrapidController
    {
        [Route("dashboard/my/apps")]
        [RestrictAnonymous]
        public async Task<ActionResult> GetAppsAsync()
        {
            int userId = (await AppUsers.GetCurrentAsync().ConfigureAwait(false)).UserId;
            int officeId = (await AppUsers.GetCurrentAsync().ConfigureAwait(true)).OfficeId;
            
            string culture = CultureManager.GetCurrent().TwoLetterISOLanguageName;
            string tenant = AppUsers.GetTenant();

            return this.Ok(App.GetAsync(tenant, userId, officeId, culture));
        }
    }
}