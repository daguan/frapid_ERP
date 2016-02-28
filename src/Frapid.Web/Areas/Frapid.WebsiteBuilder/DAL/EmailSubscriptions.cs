using Frapid.DataAccess;

namespace Frapid.WebsiteBuilder.DAL
{
    public class EmailSubscriptions
    {
        public static bool Add(string catalog, string email)
        {
            const string sql = "SELECT website.add_email_subscription(@0);";
            return Factory.Scalar<bool>(catalog, sql, email);
        }

        public static bool Remove(string catalog, string email)
        {
            const string sql = "SELECT website.remove_email_subscription(@0);";
            return Factory.Scalar<bool>(catalog, sql, email);
        }
    }
}