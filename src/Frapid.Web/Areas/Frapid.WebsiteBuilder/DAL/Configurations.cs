using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Configurations
    {
        public static DTO.Configuration GetDefaultConfiguration()
        {
            using (var db = DbProvider.Get(ConnectionString.GetConnectionString(AppUsers.GetTenant())).GetDatabase())
            {
                return db.FetchBy<DTO.Configuration>(sql => sql.Where(c => c.IsDefault)).FirstOrDefault();
            }
        }
    }
}