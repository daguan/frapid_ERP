using System.Linq;
using Frapid.Account.InputModels;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class ResetRequests
    {
        public static DTO.Reset GetIfActive(string token)
        {
            const string sql =
                "SELECT * FROM account.reset_requests WHERE request_id=@0::uuid AND expires_on >= NOW() AND NOT confirmed;";
            return Factory.Get<DTO.Reset>(AppUsers.GetTenant(), sql, token).FirstOrDefault();
        }

        public static void CompleteReset(string requestId, string password)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.complete_reset", new[] {"@0", "@1"});
            Factory.NonQuery(AppUsers.GetTenant(), sql, requestId, password);
        }

        public static DTO.Reset Request(ResetInfo model)
        {
            string sql = FrapidDbServer.GetProcedureCommand("account.reset_account", new[] { "@0", "@1", "@2" });
            return
                Factory.Get<DTO.Reset>(AppUsers.GetTenant(), sql, model.Email, model.Browser, model.IpAddress)
                    .FirstOrDefault();
        }

        public static bool HasActiveResetRequest(string email)
        {
            const string sql = "SELECT account.has_active_reset_request(@0::text);";
            return Factory.Scalar<bool>(AppUsers.GetTenant(), sql, email);
        }
    }
}