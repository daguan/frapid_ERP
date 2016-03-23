using System.Collections.Generic;
using System.Linq;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;
using Mapster;
using Content = Frapid.WebsiteBuilder.ViewModels.Content;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ContentModel
    {
        public static List<Content> GetBlogContents()
        {
            var contents = Contents.GetBlogContents().ToList();

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