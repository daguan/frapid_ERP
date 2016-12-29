using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.Mapper;
using Frapid.Mapper.Database;
using Frapid.Mapper.Query.Insert;
using Frapid.Mapper.Query.Select;
using Frapid.Mapper.Query.Update;
using MixERP.Social.DTO;

namespace MixERP.Social.DAL
{
    public static class Feeds
    {
        public static async Task<IEnumerable<FeedItem>> GetFeedsAsync(string tenant, int userId, long lastFeedId, long parentFeedId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                string sql = "SELECT * FROM social.get_next_top_feeds(@0::integer, @1::bigint, @2::bigint);";

                if (db.DatabaseType == DatabaseType.SqlServer)
                {
                    sql = "SELECT * FROM social.get_next_top_feeds(@0, @1, @2);";
                }

                return await db.SelectAsync<FeedItem>(sql, userId, lastFeedId, parentFeedId).ConfigureAwait(false);
            }
        }

        public static async Task<FeedItem> PostAsync(string tenant, Feed model)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var feedId = await db.InsertAsync(model).ConfigureAwait(false);

                var sql = new Sql("SELECT feed_id, event_timestamp, formatted_text, created_by, " +
                                  "account.get_name_by_user_id(created_by) AS created_by_name, attachments, " +
                                  "scope, is_public, parent_feed_id");

                sql.Append("FROM social.feeds");
                sql.Where("deleted = @0", false);
                sql.And("feed_id = @0", feedId);

                var awaiter = await db.SelectAsync<FeedItem>(sql).ConfigureAwait(false);

                return awaiter.FirstOrDefault();
            }
        }

        public static async Task<Feed> GetFeedAsync(string tenant, long feedId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var sql = new Sql("SELECT * FROM social.feeds");
                sql.Where("deleted = @0", false);
                sql.And("feed_id=@0", feedId);

                var awatier = await db.SelectAsync<Feed>(sql).ConfigureAwait(false);
                return awatier.FirstOrDefault();
            }
        }

        public static async Task DeleteAsync(string tenant, Feed feed, LoginView meta)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                feed.AuditTs = DateTimeOffset.UtcNow;
                feed.Deleted = true;
                feed.DeletedOn = DateTimeOffset.UtcNow;
                feed.DeletedBy = meta.UserId;

                await db.UpdateAsync(feed, feed.FeedId).ConfigureAwait(false);
            }
        }
    }
}