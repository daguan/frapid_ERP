using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frapid.Configuration;
using Frapid.Framework;
using Frapid.Framework.Extensions;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.Syndication.Rss
{
    public static class RssModel
    {
        public static RssChannel GetRssChannel(HttpContextBase context, string categoryAlias, int pageNumber)
        {
            string title = Configuration.Get("BlogTitle");
            string copyright = Configuration.Get("RssCopyrightText");
            int ttl = Configuration.Get("RssTtl").To<int>();
            int limit = Configuration.Get("RssPageSize").To<int>();
            int offset = (pageNumber - 1)*limit;

            List<PublishedContentView> contents;
            string category = string.Empty;

            if (!string.IsNullOrWhiteSpace(categoryAlias))
            {
                contents = Contents.GetBlogContents(categoryAlias, limit, offset).ToList();
                category = contents.Select(x => x.CategoryName).FirstOrDefault();
            }
            else
            {
                contents = Contents.GetBlogContents(limit, offset).ToList();
            }


            string domain = DbConvention.GetBaseDomain(context, true);

            var items = contents.Select(view => new RssItem
            {
                Title = view.Title,
                Description = view.Contents,
                Link = UrlHelper.CombineUrl(domain, "/blog/" + view.CategoryAlias + "/" + view.Alias),
                Ttl = ttl,
                Category = view.CategoryName,
                PublishDate = view.PublishOn,
                LastBuildDate = view.LastEditedOn
            }).ToList();

            var channel = new RssChannel
            {
                Title = title,
                Description = "Category: " + category,
                Link = UrlHelper.CombineUrl(domain, "/blog"),
                Items = items,
                Copyright = copyright,
                Category = category
            };

            return channel;
        }
    }
}