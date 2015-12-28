using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Frapid.Areas;
using Frapid.Account.DAL;
using Frapid.Account.DTO;
using Frapid.Account.Emails;
using Frapid.Account.InputModels;
using Frapid.Account.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Frapid.Account.RemoteAuthentication
{
    public class GoogleAuthentication
    {
        private const string ProviderName = "Google";

        public GoogleAuthentication()
        {
            var profile = DAL.ConfigurationProfiles.GetActiveProfile();
            ClientId = profile.GoogleSigninClientId;
        }

        public string ClientId { get; set; }
        //https://developers.google.com/identity/sign-in/web/backend-auth
        private async Task<bool> ValidateAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            using (var client = new HttpClient())
            {
                string url = "https://www.googleapis.com";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("/oauth2/v3/tokeninfo?id_token=" + token);
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    string aud = result["aud"].ToString();

                    if (aud == ClientId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<LoginResult> AuthenticateAsync(GoogleAccount account, RemoteUser user)
        {
            bool validationResult = Task.Run(() => ValidateAsync(account.Token)).Result;

            if (!validationResult)
            {
                return new LoginResult
                {
                    Status = false,
                    Message = "Access is denied"
                };
            }

            var gUser = new GoogleUserInfo
            {
                Email = account.Email,
                Name = account.Name
            };
            var result = GoogleSignIn.SignIn(account.Email, account.OfficeId, account.Name, account.Token, user.Browser, user.IpAddress, account.Culture);

            if (result.Status)
            {
                if (!Registrations.HasAccount(account.Email))
                {
                    string template = "~/Catalogs/{catalog}/Areas/Frapid.Account/EmailTemplates/welcome-email-other.html";
                    var welcomeEmail = new WelcomeEmail(gUser, template, ProviderName);
                    await welcomeEmail.SendAsync();
                }
            }

            return result;
        }
    }
}