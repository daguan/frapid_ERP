using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.DataAccess;

namespace Frapid.ApplicationState.Cache
{
    public static class MetaLoginHelper
    {
        public static LoginView Get(string database, long loginId)
        {
            const string sql = "SELECT * FROM account.sign_in_view WHERE login_id=@0;";
            return Factory.Get<LoginView>(database, sql, loginId).FirstOrDefault();
        }
    }
}