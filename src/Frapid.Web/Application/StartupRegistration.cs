using System;
using System.Linq;
using Frapid.Framework;
using Frapid.Web.Jobs;
using Serilog;

namespace Frapid.Web
{
    public class StartupRegistration
    {
        public static void Register()
        {
            try
            {
                var iType = typeof(IStartupRegistration);
                var members = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance);

                foreach (IStartupRegistration member in members)
                {
                    try
                    {
                        member.Register();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Could not register the startup job \"{Description}\" due to errors. Exception: {Exception}", member.Description, ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex);
                throw;
            }

        }
    }
}