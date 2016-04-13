using System.Linq;
using Frapid.Account.DTO;
using Frapid.Account.InputModels;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.Account.DAL
{
    public static class ResetRequests
    {
        public static Reset GetIfActive(string tenant, string token)
        {
            const string sql =
                "SELECT * FROM account.reset_requests WHERE request_id=@0::uuid AND expires_on >= NOW() AND NOT confirmed;";
            return Factory.Get<Reset>(tenant, sql, token).FirstOrDefault();
        }

        public static void CompleteReset(string tenant, string requestId, string password)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "account.complete_reset", new[] {"@0", "@1"});
            Factory.NonQuery(tenant, sql, requestId, password);
        }

        public static Reset Request(string tenant, ResetInfo model)
        {
            string sql = FrapidDbServer.GetProcedureCommand(tenant, "account.reset_account", new[] {"@0", "@1", "@2"});
            return
                Factory.Get<Reset>(tenant, sql, model.Email, model.Browser, model.IpAddress)
                    .FirstOrDefault();
        }

        public static bool HasActiveResetRequest(string tenant, string email)
        {
            const string sql = "SELECT account.has_active_reset_request(@0::text);";
            return Factory.Scalar<bool>(tenant, sql, email);
        }
    }
}