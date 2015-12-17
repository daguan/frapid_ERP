using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class MyTemplateController : Controller
    {
        [Route("my/template/{*resource}")]
        public ActionResult Get(string resource = "")
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                return HttpNotFound();
            }

            string directory = HostingEnvironment.MapPath(Configuration.GetCurrentThemePath());

            if (directory == null)
            {
                return HttpNotFound();
            }

            string path = Path.Combine(directory, resource);

            if (!System.IO.File.Exists(path))
            {
                return HttpNotFound();
            }

            string mimeType = GetMimeType(path);

            return File(path, mimeType);
        }

        private string GetMimeType(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }
    }
}