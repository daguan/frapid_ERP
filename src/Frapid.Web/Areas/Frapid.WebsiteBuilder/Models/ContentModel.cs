using AutoMapper;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.DTO;
using Content = Frapid.WebsiteBuilder.ViewModels.Content;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ContentModel
    {
        public static Content GetContent(string categoryAlias = "", string alias = "")
        {
            var content = Contents.GetPublished(categoryAlias, alias);
            if (content == null)
            {
                return null;
            }

            Mapper.CreateMap<PublishedContentView, Content>();
            var model = Mapper.Map<Content>(content);
            return model;
        }
    }
}