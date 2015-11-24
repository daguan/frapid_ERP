using System.Collections.Generic;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace WebsiteBuilder.DAL
{
    public class Module
    {
        public static Models.Module Get(string alias)
        {
            const string sql = "SELECT * FROM wb.modules WHERE alias=@0;";
            return Factory.Single<Models.Module>(AppUsers.GetCatalog(), sql, alias);
        }

        public static IEnumerable<Models.Module> Get()
        {
            const string sql = "SELECT * FROM wb.modules;";
            return Factory.Get<Models.Module>(AppUsers.GetCatalog(), sql);
        }
    }
}