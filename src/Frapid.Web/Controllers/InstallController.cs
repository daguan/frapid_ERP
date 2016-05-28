using System.Linq;
using System.Web.Mvc;
using Frapid.Areas;
using Frapid.Configuration;
using Frapid.Installer;

namespace Frapid.Web.Controllers
{
    public class InstallController: FrapidController
    {
        [Route("install")]
        public ActionResult Index()
        {
            string domain = TenantConvention.GetDomain();

            var approved = new DomainSerializer("DomainsApproved.json");
            var installed = new DomainSerializer("DomainsInstalled.json");

            if(!approved.GetMemberSites().Any(x => x.Equals(domain)))
            {
                return this.HttpNotFound();
            }

            if(installed.GetMemberSites().Any(x => x.Equals(domain)))
            {
                return this.Redirect("/");
            }

            var setup = approved.Get().FirstOrDefault(x => x.GetSubtenants().Contains(domain.ToLowerInvariant()));
            InstallationFactory.Setup(setup); //Background job
            return this.Content("Installing frapid, please visit the site after a few minutes.");
        }
    }
}