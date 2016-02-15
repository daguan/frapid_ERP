using System;
using System.Linq;
using Frapid.Framework;
using Quartz;
using Serilog;

namespace Frapid.Web.Jobs
{
    public class EndOfDayJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var iType = typeof(IDayEndTask);
                var members = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance);

                foreach (IDayEndTask member in members)
                {
                    try
                    {
                        member.Register();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(
                            "Could not register the EOD job \"{Description}\" due to errors. Exception: {Exception}",
                            member.Description, ex);
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