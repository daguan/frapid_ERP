using System;
using System.Collections.Generic;
using System.Linq;

namespace Frapid.Framework.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<T> GetTypeMembers<T>(this Type iType)
        {
            var members = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(x => x.GetTypes())
                                   .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface)
                                   .Select(Activator.CreateInstance);

            return members.Cast<T>();
        }

        public static IEnumerable<T> GetTypeMembersNotAbstract<T>(this Type iType)
        {
            var members = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(x => x.GetTypes())
                                   .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                   .Select(Activator.CreateInstance);

            return members.Cast<T>();
        }
    }
}