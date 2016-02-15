using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;

namespace Frapid.Framework.Extensions
{
    public static class RequestExtensions
    {
        public static string GetClientIpAddress(this HttpContextBase context)
        {
            string ip = context?.Request?.UserHostAddress != null ? IPAddress.Parse(context.Request.UserHostAddress).ToString() : null;

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }

            return string.Empty;
        }

        public static string GetClientIpAddress(this IRequest request)
        {
            object ipAddress;
            if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
            {
                return ipAddress as string;
            }
            return null;
        }

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextBase;
            string ip = context?.Request?.UserHostAddress != null ? IPAddress.Parse(context.Request.UserHostAddress).ToString() : null;

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }

            var owinContext = request.Properties["MS_OwinContext"] as OwinContext;
            return owinContext?.Request?.RemoteIpAddress != null ? IPAddress.Parse(owinContext.Request.RemoteIpAddress).ToString() : null;
        }

        public static string GetUserAgent(this HttpContextBase context)
        {
            string ip = context?.Request?.UserAgent;

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }

            return string.Empty;
        }

        public static string GetUserAgent(this HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextBase;
            string ip = context?.Request?.UserAgent;

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }

            var owinContext = request.Properties["MS_OwinContext"] as OwinContext;
            return owinContext?.Request?.Headers.Get("User-Agent");
        }

        public static T ReadClaim<T>(this HttpRequestContext request, string type)
        {
            var principal = request.Principal as ClaimsPrincipal;
            var claim = principal?.Claims.FirstOrDefault(x => x.Type.Equals(type));

            return claim == null ? default(T) : claim.Value.To<T>();
        }

        public static T ReadClaim<T>(this HttpContextBase context, string type)
        {
            var principal = context.User as ClaimsPrincipal;
            var claim = principal?.Claims.FirstOrDefault(x => x.Type.Equals(type));

            return claim == null ? default(T) : claim.Value.To<T>();
        }

        //Web API
        public static string GetBearerToken(this HttpRequestMessage request)
        {
            var authHead = request.Headers.Authorization;
            return authHead?.ToString().Replace("Bearer ", "") ?? string.Empty;
        }

        //ASP.net MVC
        public static string GetClientToken(this HttpRequestBase request)
        {
            if (!request.Cookies.AllKeys.Contains("access_token"))
            {
                return string.Empty;
            }

            var cookie = request.Cookies["access_token"];
            return cookie == null ? string.Empty : cookie.Value;
        }

        public static string GetClientToken(this IRequest request)
        {
            if (!request.Cookies.ContainsKey("access_token"))
            {
                return string.Empty;
            }

            var cookie = request.Cookies["access_token"];
            return cookie == null ? string.Empty : cookie.Value;
        }
    }
}