using System.IO;
using System.Web.Mvc;
using Frapid.Configuration;
using Frapid.Dashboard.Controllers;

namespace Frapid.Calendar.Controllers
{
    public class CalendarBackendController : DashboardController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string baseDirectory = this.GetDirectoryName();

            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
        }

        private string GetDirectoryName()
        {
            string directory = $"/Backups/{this.Tenant}";
            return PathMapper.MapPath(directory);
        }
    }
}