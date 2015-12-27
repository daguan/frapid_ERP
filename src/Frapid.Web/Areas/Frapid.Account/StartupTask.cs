using System.Linq;
using Frapid.Account.DataAccess;
using Frapid.Configuration;
using Frapid.Framework;
using Npgsql;
using Serilog;

namespace Frapid.Account
{
    public class StartupTask : IStartupRegistration
    {
        public void Register()
        {
            try
            {
                var installed = new DomainSerializer("domains-installed.json");

                foreach (var repository in
                    from domain in installed.Get()
                    let catalog = DbConvention.GetDbNameByConvention(domain.DomainName)
                    select new AddInstalledDomainProcedure
                    {
                        DomainName = domain.DomainName,
                        AdminEmail = domain.AdminEmail,
                        SkipValidation = true,
                        _Catalog = catalog
                    })
                {
                    repository.Execute();
                }
            }
            catch (NpgsqlException ex)
            {
                Log.Error("Could not execute AddInstalledDomainProcedure. Exception: {Exception}", ex);
            }
        }
    }
}