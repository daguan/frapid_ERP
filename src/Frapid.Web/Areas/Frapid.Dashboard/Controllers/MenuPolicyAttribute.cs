using System.Linq;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Dashboard.DAL;

namespace Frapid.Dashboard.Controllers
{
    public class MenuPolicyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string path = filterContext.HttpContext.Request.FilePath;

            int userId = AppUsers.GetCurrent().UserId;
            int officeId = AppUsers.GetCurrent().OfficeId;
            string culture = AppUsers.GetCurrent().Culture;

            var policy = Menu.Get(userId, officeId, culture);

            if (!policy.Any(x => x.Url.Equals(path)))
            {
                filterContext.Result = new HttpUnauthorizedResult("Access is denied.");
            }
        }
    }
}