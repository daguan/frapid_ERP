using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;
using Content = Frapid.WebsiteBuilder.ViewModels.Content;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ContentModel
    {
        public static List<Content> GetBlogContents()
        {
            var contents = Contents.GetBlogContents();
            if (contents == null || !contents.Any())
            {
                return null;
            }

            Mapper.CreateMap<PublishedContentView, Content>();
            var model = Mapper.Map<List<Content>>(contents);
            return model;
        }


        public static Content GetContent(string tenant, string categoryAlias = "", string alias = "", bool isBlog = false)
        {
            var content = Contents.GetPublished(tenant, categoryAlias, alias, isBlog);
            if (content == null)
            {
                return null;
            }

            Mapper.CreateMap<PublishedContentView, Content>();
            var model = Mapper.Map<Content>(content);
            return model;
        }

        public static void AddHit(string database, int contentId)
        {
            Contents.AddHit(database, contentId);
        }
    }
}