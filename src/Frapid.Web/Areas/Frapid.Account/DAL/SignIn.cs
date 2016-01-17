using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public class SignIn
    {
        public static LoginResult Do(string email, int officeId, string browser, string ipAddress, string culture)
        {
            const string sql = "SELECT * FROM account.sign_in(@0::text, @1::integer, @2::text, @3::text, @4::text);";
            return Factory.Get<LoginResult>(AppUsers.GetCatalog(), sql, email, officeId, browser, ipAddress, culture).FirstOrDefault();
        }
    }
}