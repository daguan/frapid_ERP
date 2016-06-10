using System;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework;
using Serilog;

namespace Frapid.Account
{
    [Obsolete]
    public static class InstalledDomains
    {
        public static async Task AddAsync(string database, string domainName, string adminEmail)
        {
            string sql = FrapidDbServer.GetProcedureCommand(database, "account.add_installed_domain", new[] {"@0", "@1"});
            await Factory.NonQueryAsync(database, sql, domainName, adminEmail).ConfigureAwait(false);
        }
    }

    public class StartupTask : IStartupRegistration
    {
        public string Description { get; set; } = "Upserting installed domains to the DB.";

        public async Task RegisterAsync()
        {
            try
            {
                var installed = new InstalledDomainSerializer();

                foreach (var domain in installed.Get())
                {
                    string database = TenantConvention.GetDbNameByConvention(domain.DomainName);
                    await InstalledDomains.AddAsync(database, domain.DomainName, domain.AdminEmail).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not execute AddInstalledDomainProcedure. Exception: {Exception}", ex);
            }
        }
    }
}