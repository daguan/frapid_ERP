using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.WebsiteBuilder.DAL;
using Frapid.WebsiteBuilder.ViewModels;
using Mapster;

namespace Frapid.WebsiteBuilder.Models
{
    public static class ContentModel
    {
        public static async Task<IEnumerable<Content>> GetBlogContentsAsync(int pageNumber)
        {
            int pageSize = 10;
            int offset = (pageNumber - 1) * pageSize;
            string tenant = AppUsers.GetTenant();

            var awaiter = await Contents.GetBlogContentsAsync(tenant, pageSize, offset);
            var contents = awaiter.ToList();

            if (!contents.Any())
            {
                return null;
            }

            var model = contents.Adapt<IEnumerable<Content>>();
            return model;
        }


        public static async Task<Content> GetContentAsync(string tenant, string categoryAlias = "", string alias = "",
            bool isBlog = false)
        {
            var content = await Contents.GetPublishedAsync(tenant, categoryAlias, alias, isBlog);

            var model = content?.Adapt<Content>();
            return model;
        }

        internal static async Task AddHitAsync(string database, string categoryAlias, string alias)
        {
            await Contents.AddHitAsync(database, categoryAlias, alias);
        }
    }
}