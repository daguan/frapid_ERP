using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.CacheFactory;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.Contracts;
using Frapid.WebsiteBuilder.ViewModels;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ErrorModel
    {
        public static List<IContentSearch> GetCandidates()
        {
            var iType = typeof(IContentSearch);
            var members = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance);
            return members.Cast<IContentSearch>().ToList();
        }

        private static string Sanitize(string query)
        {
            query = query.Or("").Split('/').Last();

            query = query.Replace(".html", "").Replace(".htm", "").Replace("-", " ").Replace("_", " ");
            query = new string(query.Where(x => char.IsLetter(x) || char.IsWhiteSpace(x)).ToArray()).Trim();
            return query;
        }

        public static SearchResult GetResult(string query)
        {
            query = Sanitize(query);
            string key = "/search-contents/" + query;
            var factory = new DefaultCacheFactory();

            var result = factory.Get<SearchResult>(key);

            if (result == null)
            {
                result = FromStore(query);
                factory.Add(key, result, DateTimeOffset.UtcNow.AddMinutes(15));
            }

            return result;
        }

        public static SearchResult FromStore(string query)
        {
            var contents = new List<SearchResultContent>();

            var candidates = GetCandidates();

            foreach (var candidate in candidates)
            {
                var items = candidate.Search(query);
                contents.AddRange(items);
            }

            return new SearchResult
            {
                Query = query,
                Contents = contents.OrderByDescending(x => x.LastUpdatedOn).Take(20).ToList()
            };
        }
    }
}