using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.DataAccess;

namespace Frapid.TokenManager.DAL
{
    public class AccessTokens
    {
        public static async Task<bool> IsValidAsync(string clientToken, string ipAddress, string userAgent)
        {
            const string sql = "SELECT account.is_valid_client_token(@0, @1, @2);";
            return await Factory.ScalarAsync<bool>(TenantConvention.GetTenant(), sql, clientToken, ipAddress, userAgent);
        }
    }
}