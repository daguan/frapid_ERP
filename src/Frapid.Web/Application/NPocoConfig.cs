using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Web
{
    public static class NPocoConfig
    {
        public static void Register()
        {
            DbProvider.Setup(typeof(IPoco));
        }
    }
}