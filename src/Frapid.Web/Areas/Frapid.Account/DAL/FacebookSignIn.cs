using System.Linq;
using Frapid.Account.DTO;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.Framework.Extensions;

namespace Frapid.Account.DAL
{
    public static class FacebookSignIn
    {
        public static LoginResult SignIn(string tenant, string facebookUserId, string email, int officeId, string name,
            string token, string browser,
            string ipAddress, string culture)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "account.fb_sign_in",
                new[] {"@0", "@1", "@2", "@3", "@4", "@5", "@6", "@7"});
            return Factory.Get<LoginResult>(tenant, sql, facebookUserId, email, officeId, name, token, browser,
                ipAddress, culture.Or("en-US")).FirstOrDefault();
        }
    }
}