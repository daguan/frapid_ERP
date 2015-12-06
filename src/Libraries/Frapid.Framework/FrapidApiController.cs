using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Frapid.Framework
{
    public class FrapidApiController : ApiController
    {
        public static List<Assembly> GetMembers()
        {
            Type type = typeof(FrapidApiController);
            try
            {
                List<Assembly> items = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.IsSubclassOf(type)).Select(t => t.Assembly).ToList();
                return items;

            }
            catch (ReflectionTypeLoadException)
            {
                //Swallow the exception
            }

            return null;
        }
    }
}