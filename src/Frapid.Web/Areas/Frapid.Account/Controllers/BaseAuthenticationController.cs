using System;
using System.Net;
using System.Security;
using System.Threading;
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
        protected bool CheckPassword(string email, string plainPassword)
        {
            var user = Users.Get(email);

            if (user == null)
            {
                return false;
            }

            return PasswordManager.ValidateBcrypt(plainPassword, user.Password);
        }

        protected ActionResult OnAuthenticated(LoginResult result, SignInInfo model = null)
        {
            if (!result.Status)
            {
                Thread.Sleep(new Random().Next(1, 5)*1000);
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, JsonConvert.SerializeObject(result));
            }

            Guid? applicationId = null;

            if (model != null)
            {
                applicationId = model.ApplicationId;
            }

            var manager = new Provider(AppUsers.GetTenant(), applicationId, result.LoginId);
            var token = manager.GetToken();
            string domain = DbConvention.GetDomain();

            AccessTokens.Save(token, this.RemoteUser.IpAddress, this.RemoteUser.UserAgent);

            var cookie = new HttpCookie("access_token")
            {
                Value = token.ClientToken,
                HttpOnly = true,
                Secure = true,
                Expires = token.ExpiresOn
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