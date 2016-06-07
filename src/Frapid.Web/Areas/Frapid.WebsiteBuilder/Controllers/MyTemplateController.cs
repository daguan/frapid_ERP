using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Areas.Caching;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.Controllers
{
    public class MyTemplateController : FrapidController
    {
        [Route("my/template/{*resource}")]
        [FileOutputCache(Duration = 60 * 24 * 7, SlidingExpiration = true)]
        public ActionResult Get(string resource = "")
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                return HttpNotFound();
            }

            var allowed = FrapidConfig.GetMyAllowedResources(TenantConvention.GetTenant());

            if (string.IsNullOrWhiteSpace(resource) || allowed.Count().Equals(0))
            {
                return this.HttpNotFound();
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

            string extension = Path.GetExtension(path);

            if (!allowed.Contains(extension))
            {
                return this.HttpNotFound();
            }

            string mimeType = this.GetMimeType(path);
            return this.File(path, mimeType);
        }

        private string GetMimeType(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }
    }
}