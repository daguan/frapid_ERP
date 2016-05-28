using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public static class Contents
    {
        public static async Task<IEnumerable<Content>> GetContentsAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Content>().Where(c => c.IsHomepage).ToListAsync();
            }
        }

        public static async Task<IEnumerable<PublishedContentView>> GetAllPublishedContentsAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<PublishedContentView>().ToListAsync();
            }
        }

        public static async Task<Content> GetAsync(string tenant, int contentId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Content>().Where(c => c.ContentId == contentId)
                    .FirstOrDefaultAsync();
            }
        }

        public static async Task<PublishedContentView> GetPublishedAsync(string tenant, string categoryAlias, string alias,
            bool isBlog)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return await GetDefaultAsync(tenant);
            }

            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<PublishedContentView>().Where(c => c.Alias.ToLower().Equals(alias.ToLower())
                                                                         &&
                                                                         c.CategoryAlias.ToLower()
                                                                             .Equals(categoryAlias.ToLower())
                                                                         && c.IsBlog == isBlog
                    )
                    .FirstOrDefaultAsync();
            }
        }


        public static async Task<IEnumerable<PublishedContentView>> GetBlogContentsAsync(string tenant, string categoryAlias,
            int limit,
            int offset)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await
                    db.Query<PublishedContentView>().Where(x => x.IsBlog && x.CategoryAlias == categoryAlias)
                        .Limit(offset, limit).ToListAsync();
            }
        }

        public static async Task<int> CountBlogContentsAsync(string tenant, string categoryAlias)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await
                    db.Query<PublishedContentView>().Where(x => x.IsBlog && x.CategoryAlias == categoryAlias)
                        .CountAsync();
            }
        }

        public static async Task<int> CountBlogContentsAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<PublishedContentView>().Where(x => x.IsBlog).CountAsync();
            }
        }

        public static async Task<IEnumerable<PublishedContentView>> GetBlogContentsAsync(string tenant, int limit, int offset)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    await db.Query<PublishedContentView>().Where(x => x.IsBlog).Limit(offset, limit).ToListAsync();
            }
        }

        public static async Task<PublishedContentView> GetDefaultAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await
                    db.Query<PublishedContentView>().Where(c => c.IsHomepage).Limit(1)
                        .FirstOrDefaultAsync();
            }
        }

        internal static async Task AddHitAsync(string tenant, string categoryAlias, string alias)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "website.add_hit", new[] {"@0", "@1"});
            await Factory.NonQueryAsync(tenant, sql, categoryAlias, alias);
        }

        public static async Task<IEnumerable<PublishedContentView>> SearchAsync(string tenant, string query)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await
                    db.Query<PublishedContentView>().Where(
                        c =>
                            c.Title.ToLower().Contains(query.ToLower()) ||
                            c.Alias.ToLower().Contains(query.ToLower()) ||
                            c.Contents.ToLower().Contains(query.ToLower())).ToListAsync();
            }
        }
    }
}