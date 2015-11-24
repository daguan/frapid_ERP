using Frapid.DataAccess;

namespace Frapid.Web
{
    public static class NPocoConfig
    {
        public static void Register()
        {
            Provider.Setup();
        }
    }
}