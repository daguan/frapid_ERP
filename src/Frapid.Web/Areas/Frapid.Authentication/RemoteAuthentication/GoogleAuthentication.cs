using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Frapid.Authentication.DAL;
using Frapid.Authentication.DTO;
using Frapid.Authentication.Messaging;
using Frapid.Authentication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebsiteBuilder.Models;

namespace Frapid.Authentication.RemoteAuthentication
{
    public class GoogleAuthentication
    {
        private const string ProviderName = "Google";

        public GoogleAuthentication()
        {
            ConfigurationProfile profile = Configuration.GetActiveProfile();
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

            using (HttpClient client = new HttpClient())
            {
                string url = "https://www.googleapis.com";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/oauth2/v3/tokeninfo?id_token=" + token);
                if (response.IsSuccessStatusCode)
                {
                    JObject result = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                    string aud = result["aud"].ToString();

                    if (aud == ClientId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<SignInResult> AuthenticateAsync(string email, string name, string token, RemoteUser user)
        {
            bool validationResult = Task.Run(() => ValidateAsync(token)).Result;

            if (!validationResult)
            {
                return new SignInResult
                {
                    Status = false,
                    Message = "Access is denied"
                };
            }

            GoogleUserInfo gUser = new GoogleUserInfo
            {
                Email = email,
                Name = name
            };

            string template = "~/Catalogs/{catalog}/Areas/Frapid.Authentication/EmailTemplates/welcome-3rd-party.html";

            if (!DAL.Registration.HasAccount(email))
            {
                WelcomeEmail welcomeEmail = new WelcomeEmail(gUser, template, ProviderName);
                await welcomeEmail.SendAsync();
            }

            SignInResult result = GoogleSignIn.SignIn(email, name, token, user.Browser, user.IpAddress, user.Culture);
            return result;
        }
    }
}