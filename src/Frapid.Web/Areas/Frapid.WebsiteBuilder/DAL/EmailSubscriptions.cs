using Frapid.WebsiteBuilder.DataAccess;

namespace Frapid.WebsiteBuilder.DAL
{
    public class EmailSubscriptions
    {
        public static bool Add(string catalog, string email)
        {
            var repository = new AddEmailSubscriptionProcedure(email) {_Catalog = catalog, SkipValidation = true};
            return repository.Execute();
        }

        public static bool Remove(string catalog, string email)
        {
            var repository = new RemoveEmailSubscriptionProcedure(email) {_Catalog = catalog, SkipValidation = true};
            return repository.Execute();
        }
    }
}