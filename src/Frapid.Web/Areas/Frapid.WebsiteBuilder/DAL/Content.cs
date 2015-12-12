using System;
using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.NPoco;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Content
    {
        public static IEnumerable<Entities.Content> GetContents()
        {
            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase()
                )
            {
                return db.FetchBy<Entities.Content>(sql => sql.Where(c => c.IsHomepage));
            }
        }

        public static Entities.Content Get(int contentId)
        {
            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<Entities.Content>(sql => sql
                    .Where(c => c.ContentId == contentId))
                    .FirstOrDefault();
            }
        }

        public static Entities.Content GetPublished(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return GetDefault();
            }

            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<Entities.Content>(sql => sql
                    .Where(c => c.Alias.ToLower().Equals(alias))
                    .Where(c => c.PublishOn <= DateTime.Now)
                    .Where(c => !c.IsDraft))
                    .FirstOrDefault();
            }
        }

        public static Entities.Content GetDefault()
        {
            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase()
                )
            {
                return db.FetchBy<Entities.Content>(sql => sql.Where(c => c.IsHomepage)).FirstOrDefault();
            }
        }
    }
}