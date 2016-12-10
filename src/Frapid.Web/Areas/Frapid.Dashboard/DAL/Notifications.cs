using System;
using System.Threading.Tasks;
using Frapid.Dashboard.DTO;
using Frapid.DataAccess;
using Frapid.Framework.Extensions;

namespace Frapid.Dashboard.DAL
{
    public static class Notifications
    {
        public static async Task<Guid> AddAsync(string tenant, Notification notification)
        {
            var id = await Factory.InsertAsync(tenant, notification).ConfigureAwait(false);
            return id.To<Guid>();
        }
    }
}