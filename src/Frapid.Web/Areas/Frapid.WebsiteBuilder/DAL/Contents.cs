using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public static class Contents
    {
        public static IEnumerable<Content> GetContents()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<Content>(sql => sql.Where(c => c.IsHomepage));
            }
        }

        public static IEnumerable<PublishedContentView> GetAllPublishedContents()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql);
            }
        }

        public static Content Get(int contentId)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
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
                return GetDefault();
            }

            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(tenant)).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql
                    .Where(c => c.Alias.ToLower().Equals(alias.ToLower())
                                && c.CategoryAlias.ToLower().Equals(categoryAlias.ToLower())
                                && c.IsBlog == isBlog
                    ))
                    .FirstOrDefault();
            }
        }


        public static IEnumerable<PublishedContentView> GetBlogContents(string categoryAlias, int limit, int offset)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog && x.CategoryAlias == categoryAlias))
                        .Skip(offset)
                        .Take(limit);
            }
        }

        public static int CountBlogContents(string categoryAlias)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog && x.CategoryAlias == categoryAlias))
                        .Count;
            }
        }

        public static int CountBlogContents()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog)).Count;
            }
        }

        public static IEnumerable<PublishedContentView> GetBlogContents(int limit, int offset)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql.Where(x => x.IsBlog)).Skip(offset).Take(limit);
            }
        }

        public static PublishedContentView GetDefault()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(c => c.IsHomepage).Limit(1))
                        .FirstOrDefault();
            }
        }

        internal static void AddHit(string database, string categoryAlias, string alias)
        {
            string sql;
            if (string.IsNullOrWhiteSpace(categoryAlias) && string.IsNullOrWhiteSpace(alias))
            {
                sql = "UPDATE website.contents SET hits = COALESCE(website.contents.hits, 0) + 1 " +
                      "WHERE is_homepage = true;";
                Factory.NonQuery(database, sql, categoryAlias, alias);
                return;
            }

            sql = "UPDATE website.contents SET hits = COALESCE(website.contents.hits, 0) + 1 " +
                  "WHERE website.contents.content_id=(SELECT website.published_content_view.content_id FROM website.published_content_view " +
                  "WHERE category_alias=@0 AND alias=@1);";
            Factory.NonQuery(database, sql, categoryAlias, alias);
        }

        public static List<PublishedContentView> Search(string query)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
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