using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Contacts
    {
        public static async Task<IEnumerable<Contact>> GetContactsAsync(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Contact>().Where(c => c.Status)
                    .OrderBy(c => c.Sort)
                    .ThenBy(c => c.ContactId).ToListAsync();
            }
        }

        public static async Task<Contact> GetContactAsync(string tenant, int contactId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return await db.Query<Contact>().Where(c => c.ContactId.Equals(contactId)).FirstOrDefaultAsync();
            }
        }
    }
}