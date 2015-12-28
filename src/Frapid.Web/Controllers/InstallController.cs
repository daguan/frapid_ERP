using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Installer;
using System.Linq;

namespace Frapid.Web.Controllers
{
    public class InstallController : FrapidController
    {
        [Route("install")]
        public ActionResult Index()
        {
            string domain = DbConvention.GetDomain();

            var approved = new DomainSerializer("DomainsApproved.json");
            var installed = new DomainSerializer("DomainsInstalled.json");
           
            if (!approved.Get().Any(x => x.DomainName.Equals(domain)))
            {
                return this.HttpNotFound();
            }

            if (installed.Get().Any(x => x.DomainName.Equals(domain)))
            {
                return this.Redirect("/");
            }

            var setup = approved.Get().FirstOrDefault(x => x.DomainName.Equals(domain));
            InstallationFactory.Setup(setup); //Background job
            return this.Content("Installing frapid, please visit the site after a few minutes.");
        }
    }
}