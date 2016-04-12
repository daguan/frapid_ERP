using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.WebsiteBuilder.DAL
{
    public class EmailSubscriptions
    {
        public static bool Add(string database, string email)
        {
            string sql = FrapidDbServer.GetProcedureCommand("website.add_email_subscription", new[] {"@0"});
            return Factory.Scalar<bool>(database, sql, email);
        }

        public static bool Remove(string database, string email)
        {
            string sql = FrapidDbServer.GetProcedureCommand("website.remove_email_subscription", new[] {"@0"});
            return Factory.Scalar<bool>(database, sql, email);
        }
    }
}