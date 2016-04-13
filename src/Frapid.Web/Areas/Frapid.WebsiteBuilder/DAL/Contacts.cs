using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.WebsiteBuilder.DTO;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Contacts
    {
        public static IEnumerable<Contact> GetContacts(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Contact>(sql => sql.Where(c => c.Status))
                    .OrderBy(c => c.Sort)
                    .ThenBy(c => c.ContactId);
            }
        }

        public static Contact GetContact(string tenant, int contactId)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<Contact>(sql => sql.Where(c => c.ContactId.Equals(contactId))).FirstOrDefault();
            }
        }
    }
}