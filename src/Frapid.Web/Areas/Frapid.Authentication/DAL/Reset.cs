using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Authentication.ViewModels;
using Frapid.DataAccess;

namespace Frapid.Authentication.DAL
{
    public static class Reset
    {
        public static DTO.Reset GetIfActive(string token)
        {
            const string sql = "SELECT * FROM auth.reset_requests WHERE request_id=@0::uuid AND expires_on >= NOW() AND NOT confirmed;";
            return Factory.Get<DTO.Reset>(AppUsers.GetCatalog(), sql, token).FirstOrDefault();
        }

        public static void CompleteReset(string requestId, string password)
        {
            const string sql = "SELECT * FROM auth.complete_reset(@0::uuid, @1::text)";
            Factory.NonQuery(AppUsers.GetCatalog(), sql, requestId, password);
        }

        public static DTO.Reset Request(ResetInputModel model)
        {
            const string sql = "SELECT * FROM auth.reset_account(@0::text, @1::text, @2::text);";
            return Factory.Get<DTO.Reset>(AppUsers.GetCatalog(), sql, model.Email, model.Browser, model.IpAddress).FirstOrDefault();
        }

        public static bool HasActiveResetRequest(string email)
        {
            const string sql = "SELECT * FROM auth.has_active_reset_request(@0::text);";
            return Factory.Scalar<bool>(AppUsers.GetCatalog(), sql, email);
        }
    }
}