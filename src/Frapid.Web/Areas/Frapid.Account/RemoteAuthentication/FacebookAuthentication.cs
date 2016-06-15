using System.Threading.Tasks;
using Facebook;
using Frapid.Account.DAL;
using Frapid.Account.DTO;
using Frapid.Account.Emails;
using Frapid.Account.InputModels;
using Frapid.Account.ViewModels;
using Frapid.Areas;

namespace Frapid.Account.RemoteAuthentication
{
    public class FacebookAuthentication
    {
        public string Tenant { get; }

        public FacebookAuthentication(string tenant)
        {
            this.Tenant = tenant;
        }

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

            var result =
                await
                    FacebookSignIn.SignInAsync(this.Tenant, account.FacebookUserId, account.Email, account.OfficeId,
                        facebookUser.Name, account.Token, user.Browser,
                        user.IpAddress, account.Culture).ConfigureAwait(false);

            if (result.Status)
            {
                if (!await Registrations.HasAccountAsync(this.Tenant, account.Email).ConfigureAwait(false))
                {
                    string template = "~/Tenants/{tenant}/Areas/Frapid.Account/EmailTemplates/welcome-email-other.html";
                    var welcomeEmail = new WelcomeEmail(facebookUser, template, ProviderName);
                    await welcomeEmail.SendAsync(this.Tenant).ConfigureAwait(false);
                }
            }
            return result;
        }
    }
}