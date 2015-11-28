using Frapid.ApplicationState.Cache;
using Frapid.Authentication.DTO;
using Frapid.DataAccess;

namespace Frapid.Authentication.DAL
{
    public static class Configuration
    {
        public static ConfigurationProfile GetActiveProfile()
        {
            const string sql = "SELECT * FROM auth.configuration_profiles WHERE is_active;";
            return Factory.Single<ConfigurationProfile>(AppUsers.GetCatalog(), sql);
        }
    }
}