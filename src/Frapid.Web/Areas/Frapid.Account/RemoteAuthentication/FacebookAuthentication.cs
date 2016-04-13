using System.Threading.Tasks;
using Facebook;
using Frapid.Areas;
using Frapid.Account.DAL;
using Frapid.Account.DTO;
using Frapid.Account.Emails;
using Frapid.Account.InputModels;
using Frapid.Account.ViewModels;
using Frapid.ApplicationState.Cache;

namespace Frapid.Account.RemoteAuthentication
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
            var facebook = new FacebookClient(token);
            dynamic me = facebook.Get("me", new {fields = new[] {"id", "email", "name"}});

            return new FacebookUserInfo
            {
                Id = me.id,
                Name = me.name,
                Email = me.email
            };
        }

        public async Task<LoginResult> AuthenticateAsync(FacebookAccount account, RemoteUser user)
        {
            var facebookUser = GetFacebookUserInfo(account.Token);

            if (!Validate(facebookUser, account.FacebookUserId, account.Email))
            {
                return new LoginResult
                {
                    Status = false,
                    Message = "Access is denied"
                };
            }

            string tenant = AppUsers.GetTenant();

            var result = FacebookSignIn.SignIn(tenant, account.FacebookUserId, account.Email, account.OfficeId, facebookUser.Name, account.Token, user.Browser,
                user.IpAddress, account.Culture);

            if (result.Status)
            {
                if (!Registrations.HasAccount(tenant, account.Email))
                {
                    string template = "~/Tenants/{tenant}/Areas/Frapid.Account/EmailTemplates/welcome-email-other.html";
                    var welcomeEmail = new WelcomeEmail(facebookUser, template, ProviderName);
                    await welcomeEmail.SendAsync();
                }
            }
            return result;
        }
    }
}