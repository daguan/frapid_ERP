using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class Office
    {
        public static IEnumerable<DTO.Office> GetOffices()
        {
            const string sql = "SELECT office_id, office_name FROM config.offices;";
            return Factory.Get<DTO.Office>(AppUsers.GetCatalog(), sql);
        }
    }
}