using System.Dynamic;
using Frapid.Configuration;

namespace Frapid.Installer
{
    public static class DbInstalledDomains
    {
        public static void Add(ApprovedDomain tenant)
        {
            string database = DbConvention.GetDbNameByConvention(tenant.DomainName);
            using (var db = DbProvider.Get(ConnectionString.GetSuperUserConnectionString(database)).GetDatabase())
            {
                dynamic poco = new ExpandoObject();
                poco.domain_name = tenant.DomainName;
                poco.admin_email = tenant.AdminEmail;

                db.Insert("account.installed_domains", "domain_id", true, poco);
            }
        }
    }
}