using System;
using System.Linq;
using Frapid.WebsiteBuilder.Contracts;

namespace Frapid.WebsiteBuilder
{
    public static class ContentExtensions
    {
        public static string ParseHtml(string html)
        {
            var iType = typeof(IContentExtension);
            var members = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance);

            return members.Cast<IContentExtension>().Aggregate(html, (current, member) => member.ParseHtml(current));
        }
    }
}