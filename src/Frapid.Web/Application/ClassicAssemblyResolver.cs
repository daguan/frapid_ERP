using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Frapid.Framework;

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
}