using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
using Frapid.WebsiteBuilder;
using HtmlAgilityPack;
using MixERP.Social.DTO;
using Serilog;

namespace MixERP.Social.Models
{
    public static class Feeds
    {
        public static string HtmlAsText(string html)
        {
            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                return htmlDoc.DocumentNode.InnerText;
            }
            catch
            {
                Log.Warning("Invalid HTML token " + html + ".");
            }

            return string.Empty;
        }

        public static async Task<FeedItem> PostAsync(string tenant, Feed model, LoginView meta)
        {
            model.FormattedText = HtmlAsText(model.FormattedText);
            model.AuditTs = DateTimeOffset.UtcNow;
            model.CreatedBy = meta.UserId;
            model.Deleted = false;
            model.IsPublic = true;
            model.EventTimestamp = DateTimeOffset.UtcNow;

            return await DAL.Feeds.PostAsync(tenant, model).ConfigureAwait(false);
        }

        public static async Task DeleteAsync(string tenant, long feedId, LoginView meta)
        {
            var feed = await DAL.Feeds.GetFeedAsync(tenant, feedId).ConfigureAwait(false);

            if (feed == null)
            {
                return;
            }

            if (!meta.IsAdministrator)
            {
                if (feed.CreatedBy != meta.UserId)
                {
                    throw new FeedException("Access is denied");
                }
            }

            await DAL.Feeds.DeleteAsync(tenant, feed, meta).ConfigureAwait(false);
        }
    }
}