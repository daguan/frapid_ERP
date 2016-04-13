using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.ViewModels;
using Mapster;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ContentModel
    {
        public static List<Content> GetBlogContents(int pageNumber)
        {
            int pageSize = 10;
            int offset = (pageNumber - 1)*pageSize;
            string tenant = AppUsers.GetTenant();

            var contents = Contents.GetBlogContents(tenant, pageSize, offset).ToList();

            if (!contents.Any())
            {
                return null;
            }

            var model = contents.Adapt<List<Content>>();
            return model;
        }


        public static Content GetContent(string tenant, string categoryAlias = "", string alias = "",
            bool isBlog = false)
        {
            var content = Contents.GetPublished(tenant, categoryAlias, alias, isBlog);

            var model = content?.Adapt<Content>();
            return model;
        }

        internal static void AddHit(string database, string categoryAlias, string alias)
        {
            Contents.AddHit(database, categoryAlias, alias);
        }
    }
}