using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Contents
    {
        public static IEnumerable<Content> GetContents()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<Content>(sql => sql.Where(c => c.IsHomepage));
            }
        }

        public static Content Get(int contentId)
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<Content>(sql => sql
                    .Where(c => c.ContentId == contentId))
                    .FirstOrDefault();
            }
        }

        public static PublishedContentView GetPublished(string categoryAlias, string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return GetDefault();
            }

            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<PublishedContentView>(sql => sql
                    .Where(c => c.Alias.ToLower().Equals(alias.ToLower()))
                    .Where(c => c.CategoryAlias.ToLower().Equals(categoryAlias.ToLower())))
                    .FirstOrDefault();
            }
        }

        public static PublishedContentView GetDefault()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return
                    db.FetchBy<PublishedContentView>(sql => sql.Where(c => c.IsHomepage == true).Limit(1))
                        .FirstOrDefault();
            }
        }
    }
}