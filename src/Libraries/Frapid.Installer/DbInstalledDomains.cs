using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.NPoco;

namespace Frapid.Installer
{
    public static class DbInstalledDomains
    {
        public static void Add(ApprovedDomain tenant)
        {
            string database = DbConvention.GetDbNameByConvention(tenant.DomainName);

            using (var db = DbProvider.Get(FrapidDbServer.GetSuperUserConnectionString(database, database), database).GetDatabase())
            {
                var sql = new Sql("INSERT INTO account.installed_domains(domain_name, admin_email) SELECT @0, @1;", tenant.DomainName, tenant.AdminEmail);
                db.Execute(sql);
            }
        }
    }
}