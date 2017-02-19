using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas.CSRF;

namespace Frapid.AddressBook.Controllers.Backend
{
    [AntiForgery]
    public class IndexController : AddressBookBackendController
    {
        [Route("dashboard/address-book")]
        //[MenuPolicy]
        public async Task<ActionResult> IndexAsync()
        {
            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);

            return this.FrapidView(this.GetRazorView<AreaRegistration>("Backend/Index.cshtml", this.Tenant));
        }
    }
}