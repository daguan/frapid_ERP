using System;
using System.Linq;
using System.Reflection;
using Frapid.NPoco;

namespace Frapid.DataAccess
{
    public static class PropertyManager
    {
        public static object GetPropertyValue(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
    }
}