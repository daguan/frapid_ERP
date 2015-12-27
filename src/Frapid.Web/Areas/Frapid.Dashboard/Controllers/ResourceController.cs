using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;

namespace Frapid.Dashboard.Controllers
{
    [RestrictAnonymous]
    public class ResourceController : FrapidController
    {
        [Route("dashboard/resources/{*resource}")]
        public ActionResult Get(string resource = "")
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                return this.HttpNotFound();
            }

            string directory = "~/Catalogs/{0}/Areas/Frapid.Dashboard/Resources/";
            directory = string.Format(CultureInfo.InvariantCulture, directory, AppUsers.GetCatalog());
            directory = HostingEnvironment.MapPath(directory);

            if (directory == null)
            {
                return this.HttpNotFound();
            }

            string path = Path.Combine(directory, resource);

            if (!System.IO.File.Exists(path))
            {
                return this.HttpNotFound();
            }

            string mimeType = MimeMapping.GetMimeMapping(path);

            return this.File(path, mimeType);
        }
    }
}