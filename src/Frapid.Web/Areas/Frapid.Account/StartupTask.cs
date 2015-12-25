using Frapid.Account.DataAccess;
using Frapid.Configuration;
using Frapid.Framework;

namespace Frapid.Account
{
    public class StartupTask : IStartupRegistration
    {
        public void Register()
        {
            var installed = new DomainSerializer("domains-installed.json");

            foreach (var domain in installed.Get())
            {
                string catalog = DbConvention.GetDbNameByConvention(domain.DomainName);

                var repository = new AddInstalledDomainProcedure
                {
                    DomainName = domain.DomainName,
                    AdminEmail = domain.AdminEmail,
                    SkipValidation = true,
                    _Catalog = catalog
                };

                repository.Execute();
            }
        }
    }
}