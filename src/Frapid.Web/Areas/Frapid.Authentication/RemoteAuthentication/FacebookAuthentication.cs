using System.Threading.Tasks;
using Facebook;
using Frapid.Authentication.DAL;
using Frapid.Authentication.DTO;
using Frapid.Authentication.Messaging;
using Frapid.Authentication.Models;
using WebsiteBuilder.Models;

namespace Frapid.Authentication.RemoteAuthentication
{
    public class FacebookAuthentication
    {
        public string ProviderName => "Facebook";

        private bool Validate(FacebookUserInfo user, string id, string email)
        {
            return user.Id == id && user.Email == email;
        }

        private FacebookUserInfo GetFacebookUserInfo(string token)
        {
            FacebookClient facebook = new FacebookClient(token);
            dynamic me = facebook.Get("me", new { fields = new[] { "id", "email", "name" } });

            return new FacebookUserInfo
            {
                Id = me.id,
                Name = me.name,
                Email = me.email
            };
        }

        public async Task<SignInResult> AuthenticateAsync(string fbUserId, string email, string token, RemoteUser user)
        {
            FacebookUserInfo facebookUser = GetFacebookUserInfo(token);

            if (!Validate(facebookUser, fbUserId, email))
            {
                return new SignInResult
                {
                    Status = false,
                    Message = "Access is denied"
                };
            }

            string template = "~/Catalogs/{catalog}/Areas/Frapid.Authentication/EmailTemplates/welcome-3rd-party.html";

            if (!DAL.Registration.HasAccount(email))
            {
                WelcomeEmail welcomeEmail = new WelcomeEmail(facebookUser, template, ProviderName);
                await welcomeEmail.SendAsync();
            }

            SignInResult result = FacebookSignIn.SignIn(fbUserId, email, facebookUser.Name, token, user.Browser, user.IpAddress, user.Culture);
            return result;
        }

    }
}