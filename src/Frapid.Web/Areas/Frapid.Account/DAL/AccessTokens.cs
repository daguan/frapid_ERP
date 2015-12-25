using Frapid.Account.Entities;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;
using Frapid.TokenManager;
using Newtonsoft.Json;

namespace Frapid.Account.DAL
{
    public static class AccessTokens
    {
        public static void Revoke(string clientToken)
        {
            if (string.IsNullOrWhiteSpace(clientToken))
            {
                return;
            }

            const string sql =
                "UPDATE account.access_tokens SET revoked=true, revoked_on = NOW() WHERE client_token=@0;";
            Factory.NonQuery(AppUsers.GetCatalog(), sql, clientToken);
        }

        public static void Save(Token token, string ipAddress, string userAgent)
        {
            Factory.Insert(AppUsers.GetCatalog(), new AccessToken
            {
                ApplicationId = token.ApplicationId,
                Audience = token.Audience,
                Claims = JsonConvert.SerializeObject(token.Claims),
                ClientToken = token.ClientToken,
                CreatedOn = token.CreatedOn,
                ExpiresOn = token.ExpiresOn,
                Header = JsonConvert.SerializeObject(token.Header),
                IpAddress = ipAddress,
                IssuedBy = token.IssuedBy,
                LoginId = token.LoginId,
                Subject = token.Subject,
                TokenId = token.TokenId,
                UserAgent = userAgent
            });
        }
    }
}