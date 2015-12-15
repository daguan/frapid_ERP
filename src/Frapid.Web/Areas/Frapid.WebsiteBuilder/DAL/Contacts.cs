using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.NPoco;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Contacts
    {
        public static IEnumerable<Contact> GetContacts()
        {
            using (Database db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetCatalog())).GetDatabase())
            {
                return db.FetchBy<Contact>(sql => sql.Where(c => c.Status)).OrderBy(c => c.Sort).ThenBy(c=>c.ContactId);
            }
        }
    }
}