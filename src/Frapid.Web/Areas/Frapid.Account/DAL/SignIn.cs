using System.Linq;
using Frapid.Account.DTO;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public class SignIn
    {
        public static LoginResult Do(string email, int officeId, string browser, string ipAddress, string culture)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.sign_in", new[] { "@0", "@1", "@2", "@3", "@4" });
            return Factory.Get<LoginResult>(AppUsers.GetTenant(), sql, email, officeId, browser, ipAddress, culture).FirstOrDefault();
        }
    }
}