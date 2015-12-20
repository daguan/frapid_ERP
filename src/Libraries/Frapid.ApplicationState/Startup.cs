using Frapid.ApplicationState.Cache;
using Frapid.Framework;

namespace Frapid.ApplicationState
{
    public class Startup : IStartupRegistration
    {
        public void Register()
        {
            MetaLoginHelper.CreateTable();
        }
    }
}