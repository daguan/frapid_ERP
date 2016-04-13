using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Framework;
using Frapid.WebsiteBuilder.DAL;

namespace Frapid.WebsiteBuilder
{
    public sealed class SiteMap : ISiteMapGenerator
    {
        public List<SiteMapUrl> Generate()
        {
            string tenant = AppUsers.GetTenant();
            var all = Contents.GetAllPublishedContents(tenant).ToList();
            var contents = all.Where(x=>!x.IsBlog).ToList();
            var blogs = all.Where(x => x.IsBlog).ToList();
            var home = all.FirstOrDefault(x => x.IsHomepage);
            var lastBlogPost = blogs.Max(x => x.PublishOn);

            var urls = new List<SiteMapUrl>();

            if (home != null)
            {
                urls.Add(new SiteMapUrl
                {
                    Location = "/",
                    ChangeFrequency = SiteMapChangeFrequency.Weekly,
                    LastModified = home.LastEditedOn,
                    Priority = 1
                });
            }

            urls.AddRange(contents.Select(content => new SiteMapUrl
            {
                Location = "/site/" + content.CategoryAlias + "/" + content.Alias,
                ChangeFrequency = SiteMapChangeFrequency.Weekly,
                LastModified = content.LastEditedOn,
                Priority = 1
            }).ToList());

            urls.Add(new SiteMapUrl
            {
                Location = "/blog",
                ChangeFrequency = SiteMapChangeFrequency.Weekly,
                LastModified = lastBlogPost,
                Priority = 1
            });

            urls.AddRange(blogs.Select(content => new SiteMapUrl
            {
                Location = "/blog/" + content.CategoryAlias + "/" + content.Alias,
                ChangeFrequency = SiteMapChangeFrequency.Weekly,
                LastModified = content.LastEditedOn,
                Priority = 1
            }).ToList());



            return urls;
        }
    }
}