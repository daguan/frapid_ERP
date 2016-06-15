using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Frapid.Account.DAL;
using Frapid.Account.DTO;
using Frapid.Account.InputModels;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.TokenManager;
using Frapid.WebsiteBuilder.Controllers;
using Newtonsoft.Json;

namespace Frapid.Account.Controllers
{
    public class BaseAuthenticationController : WebsiteBuilderController
    {
        protected async Task<bool> CheckPasswordAsync(string tenant, string email, string plainPassword)
        {
            var user = await Users.GetAsync(tenant, email).ConfigureAwait(false);

            return user != null && PasswordManager.ValidateBcrypt(plainPassword, user.Password);
        }

        protected async Task<ActionResult> OnAuthenticatedAsync(LoginResult result, SignInInfo model = null)
        {
            if (!result.Status)
            {
                await Task.Delay(new Random().Next(1, 5)*1000).ConfigureAwait(false);
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, JsonConvert.SerializeObject(result));
            }

            Guid? applicationId = null;

            if (model != null)
            {
                applicationId = model.ApplicationId;
            }

            var loginView = await AppUsers.GetCurrentAsync(this.Tenant, result.LoginId).ConfigureAwait(false);

            var manager = new Provider(this.Tenant, applicationId, result.LoginId, loginView.UserId, loginView.OfficeId);
            var token = manager.GetToken();
            string domain = TenantConvention.GetDomain();

            await
                AccessTokens.SaveAsync(this.Tenant, token, this.RemoteUser.IpAddress, this.RemoteUser.UserAgent)
                    .ConfigureAwait(true);

            var cookie = new HttpCookie("access_token")
            {
                Value = token.ClientToken,
                HttpOnly = true,
                Expires = token.ExpiresOn.DateTime
            };

            //localhost cookie is not supported by most browsers.
            if (domain.ToLower() != "localhost")
            {
                cookie.Domain = domain;
            }

            this.Response.Cookies.Add(cookie);
            return this.Ok(token.ClientToken);
        }
    }
}