using System;
using System.Threading.Tasks;
using Frapid.Calendar.DTO;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.Framework.Extensions;
using Frapid.Mapper.Query.Insert;
using Frapid.Mapper.Query.NonQuery;
using Frapid.Mapper.Query.Update;

namespace Frapid.Calendar.DAL
{
    public static class Events
    {
        public static async Task<Guid> AddEventAsync(string tenant, Event entry)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var id = await db.InsertAsync(entry).ConfigureAwait(false);

                return id.To<Guid>();
            }
        }

        public static async Task UpdateEventAsync(string tenant, Event entry)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                await db.UpdateAsync(entry, entry.EventId).ConfigureAwait(false);
            }
        }

        public static async Task DeleteEventAsync(string tenant, Guid eventId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                const string sql = "DELETE FROM calendar.events WHERE event_id = @0";

                await db.NonQueryAsync(sql, eventId).ConfigureAwait(false);
            }
        }

    }
}