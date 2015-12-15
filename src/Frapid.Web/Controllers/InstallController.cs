using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Installer;

namespace Frapid.Web.Controllers
{
    public class InstallController : FrapidController
    {
        [Route("install")]
        public ActionResult Index()
        {
            string domain = DbConvention.GetDomain();

            var approved = new DomainSerializer("domains-approved.json");
            var installed = new DomainSerializer("domains-installed.json");

            if (!approved.Get().Contains(domain))
            {
                return this.HttpNotFound();
            }

            if (installed.Get().Contains(domain))
            {
                return this.Redirect("/");
            }

            InstallationFactory.Setup(domain); //Background job
            return this.Content("Installing frapid, please visit the site after a few minutes.");
        }
    }
}