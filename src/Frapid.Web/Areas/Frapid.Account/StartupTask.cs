using System;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework;
using Npgsql;
using Serilog;

namespace Frapid.Account
{
    public static class InstalledDomains
    {
        public static void Add(string database, string domainName, string adminEmail)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.add_installed_domain", new[] {"@0", "@1"});
            Factory.NonQuery(database, sql, domainName, adminEmail);
        }
    }

    public class StartupTask : IStartupRegistration
    {
        public string Description { get; set; } = "Upserting installed domains to the DB.";

        public void Register()
        {
            try
            {
                var installed = new DomainSerializer("DomainsInstalled.json");

                foreach (var domain in installed.Get())
                {
                    string database = DbConvention.GetDbNameByConvention(domain.DomainName);
                    InstalledDomains.Add(database, domain.DomainName, domain.AdminEmail);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Could not execute AddInstalledDomainProcedure. Exception: {Exception}", ex);
            }
        }
    }
}