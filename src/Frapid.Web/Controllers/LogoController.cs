using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Frapid.Web.Controllers
{
    public class LogoController : Controller
    {
        [Route("backend/logo")]
        public FileResult Index()
        {
            string logo = HostingEnvironment.MapPath("~/Static/images/frapid-logo.png");
            return new FileStreamResult(new FileStream(logo, FileMode.Open), "image/jpeg");
        }
    }
}