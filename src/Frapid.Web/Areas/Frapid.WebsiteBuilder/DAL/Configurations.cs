using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Configuration.Db;

namespace Frapid.WebsiteBuilder.DAL
{
    public class Configurations
    {
        public static DTO.Configuration GetDefaultConfiguration(string tenant)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                return db.FetchBy<DTO.Configuration>(sql => sql.Where(c => c.IsDefault)).FirstOrDefault();
            }
        }
    }
}