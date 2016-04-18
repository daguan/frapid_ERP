using System;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.ApplicationState.DAL
{
    public static class AppUsers
    {
        public static LoginView GetMetaLogin(string database, long loginId)
        {
            const string sql = "SELECT * FROM account.sign_in_view WHERE login_id=@0;";
            var view = Factory.Get<LoginView>(database, sql, loginId).FirstOrDefault();
            return view;
        }

        public static void UpdateActivity(string tenant, int userId, string ip, string browser)
        {
            using (var db = DbProvider.Get(FrapidDbServer.GetConnectionString(tenant), tenant).GetDatabase())
            {
                var sql = new Sql("UPDATE account.users SET ");
                sql.Append("last_seen_on = " + FrapidDbServer.GetDbTimestampFunction(tenant));
                sql.Append(",");
                sql.Append("last_ip = @0", ip);
                sql.Append(",");
                sql.Append("last_browser = @0", browser);
                sql.Where("user_id=@0", userId);

                db.Execute(sql);
            }
        }
    }
}