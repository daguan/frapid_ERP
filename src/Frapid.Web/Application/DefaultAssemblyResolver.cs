using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Frapid.Framework;
using Serilog;

namespace Frapid.Web
{
    public class ClassicAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            ICollection<Assembly> baseAssemblies = base.GetAssemblies();
            List<Assembly> assemblies = new List<Assembly>(baseAssemblies);
            List<Assembly> items = FrapidApiController.GetMembers();

            foreach (Assembly item in items)
            {
                baseAssemblies.Add(item);
            }

            return assemblies;
        }
    }

    public class DefaultAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            ICollection<Assembly> baseAssemblies = base.GetAssemblies();
            List<Assembly> assemblies = new List<Assembly>(baseAssemblies);

            try
            {
                List<Assembly> items = FrapidApiController.GetMembers();

                foreach (Assembly item in items)
                {
                    baseAssemblies.Add(item);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (Exception exception in ex.LoaderExceptions)
                {
                    Log.Error("Could not load assemblies containing Frapid Web API. Exception: {Exception}", exception);
                }
            }

            return assemblies;
        }
    }
}