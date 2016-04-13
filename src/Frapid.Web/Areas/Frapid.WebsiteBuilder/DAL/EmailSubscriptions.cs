using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.WebsiteBuilder.DAL
{
    public class EmailSubscriptions
    {
        public static bool Add(string tenant, string email)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "website.add_email_subscription", new[] {"@0"});
            return Factory.Scalar<bool>(tenant, sql, email);
        }

        public static bool Remove(string tenant, string email)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "website.remove_email_subscription", new[] {"@0"});
            return Factory.Scalar<bool>(tenant, sql, email);
        }
    }
}