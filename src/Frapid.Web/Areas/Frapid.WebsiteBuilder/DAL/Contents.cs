using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public static class Contents
    {
        public static IEnumerable<Content> GetContents(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Content>(sql => sql.Where(c => c.IsHomepage));
            }
        }

        public static IEnumerable<PublishedContentView> GetAllPublishedContents(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql);
            }
        }

        public static Content Get(string tenant, int contentId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Content>(sql => sql
                    .Where(c => c.ContentId == contentId))
                    .FirstOrDefault();
            }
        }

        public static PublishedContentView GetPublished(string tenant, string categoryAlias, string alias, bool isBlog)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return GetDefault(tenant);
            }

            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql
                    .Where(c => c.Alias.ToLower().Equals(alias.ToLower())
                                && c.CategoryAlias.ToLower().Equals(categoryAlias.ToLower())
                                && c.IsBlog == isBlog
                    ))
                    .FirstOrDefault();
            }
        }


        public static IEnumerable<PublishedContentView> GetBlogContents(string tenant, string categoryAlias, int limit, int offset)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog && x.CategoryAlias == categoryAlias))
                        .Skip(offset)
                        .Take(limit);
            }
        }

        public static int CountBlogContents(string tenant, string categoryAlias)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog && x.CategoryAlias == categoryAlias))
                        .Count;
            }
        }

        public static int CountBlogContents(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog)).Count;
            }
        }

        public static IEnumerable<PublishedContentView> GetBlogContents(string tenant, int limit, int offset)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog)).Skip(offset).Take(limit);
            }
        }

        public static PublishedContentView GetDefault(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(c => c.IsHomepage).Limit(1))
                        .FirstOrDefault();
            }
        }

        internal static void AddHit(string tenant, string categoryAlias, string alias)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "website.add_hit", new []{"@0", "@1"});
            Factory.NonQuery(tenant, sql, categoryAlias, alias);
        }

        public static List<PublishedContentView> Search(string tenant, string query)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(
                        sql =>
                            sql.Where(
                                c =>
                                    c.Title.ToLower().Contains(query.ToLower()) ||
                                    c.Alias.ToLower().Contains(query.ToLower()) ||
                                    c.Contents.ToLower().Contains(query.ToLower()))).ToList();
            }
        }
    }
}