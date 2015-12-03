using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Account.DTO;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Configuration
    {
        public static ConfigurationProfile GetActiveProfile()
        {
            const string sql = "SELECT * FROM account.configuration_profiles WHERE is_active;";
            return Factory.Get<ConfigurationProfile>(AppUsers.GetCatalog(), sql).FirstOrDefault();
        }
    }
}