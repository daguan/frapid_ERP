// ReSharper disable All
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GoogleAccessTokenRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/delete/{userId}", "DELETE", typeof(GoogleAccessTokenController), "Delete")]
        [InlineData("/api/account/google-access-token/delete/{userId}", "DELETE", typeof(GoogleAccessTokenController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/edit/{userId}", "PUT", typeof(GoogleAccessTokenController), "Edit")]
        [InlineData("/api/account/google-access-token/edit/{userId}", "PUT", typeof(GoogleAccessTokenController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/count-where", "POST", typeof(GoogleAccessTokenController), "CountWhere")]
        [InlineData("/api/account/google-access-token/count-where", "POST", typeof(GoogleAccessTokenController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/get-where/{pageNumber}", "POST", typeof(GoogleAccessTokenController), "GetWhere")]
        [InlineData("/api/account/google-access-token/get-where/{pageNumber}", "POST", typeof(GoogleAccessTokenController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/add-or-edit", "POST", typeof(GoogleAccessTokenController), "AddOrEdit")]
        [InlineData("/api/account/google-access-token/add-or-edit", "POST", typeof(GoogleAccessTokenController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/add/{googleAccessToken}", "POST", typeof(GoogleAccessTokenController), "Add")]
        [InlineData("/api/account/google-access-token/add/{googleAccessToken}", "POST", typeof(GoogleAccessTokenController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/bulk-import", "POST", typeof(GoogleAccessTokenController), "BulkImport")]
        [InlineData("/api/account/google-access-token/bulk-import", "POST", typeof(GoogleAccessTokenController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/meta", "GET", typeof(GoogleAccessTokenController), "GetEntityView")]
        [InlineData("/api/account/google-access-token/meta", "GET", typeof(GoogleAccessTokenController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/count", "GET", typeof(GoogleAccessTokenController), "Count")]
        [InlineData("/api/account/google-access-token/count", "GET", typeof(GoogleAccessTokenController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/all", "GET", typeof(GoogleAccessTokenController), "GetAll")]
        [InlineData("/api/account/google-access-token/all", "GET", typeof(GoogleAccessTokenController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/export", "GET", typeof(GoogleAccessTokenController), "Export")]
        [InlineData("/api/account/google-access-token/export", "GET", typeof(GoogleAccessTokenController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/1", "GET", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/account/google-access-token/1", "GET", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/get?userIds=1", "GET", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/account/google-access-token/get?userIds=1", "GET", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token", "GET", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/google-access-token", "GET", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/page/1", "GET", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/google-access-token/page/1", "GET", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/count-filtered/{filterName}", "GET", typeof(GoogleAccessTokenController), "CountFiltered")]
        [InlineData("/api/account/google-access-token/count-filtered/{filterName}", "GET", typeof(GoogleAccessTokenController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/get-filtered/{pageNumber}/{filterName}", "GET", typeof(GoogleAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/google-access-token/get-filtered/{pageNumber}/{filterName}", "GET", typeof(GoogleAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/google-access-token/first", "GET", typeof(GoogleAccessTokenController), "GetFirst")]
        [InlineData("/api/account/google-access-token/previous/1", "GET", typeof(GoogleAccessTokenController), "GetPrevious")]
        [InlineData("/api/account/google-access-token/next/1", "GET", typeof(GoogleAccessTokenController), "GetNext")]
        [InlineData("/api/account/google-access-token/last", "GET", typeof(GoogleAccessTokenController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/google-access-token/custom-fields", "GET", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/google-access-token/custom-fields", "GET", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/custom-fields/{resourceId}", "GET", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/google-access-token/custom-fields/{resourceId}", "GET", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/meta", "HEAD", typeof(GoogleAccessTokenController), "GetEntityView")]
        [InlineData("/api/account/google-access-token/meta", "HEAD", typeof(GoogleAccessTokenController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/count", "HEAD", typeof(GoogleAccessTokenController), "Count")]
        [InlineData("/api/account/google-access-token/count", "HEAD", typeof(GoogleAccessTokenController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/all", "HEAD", typeof(GoogleAccessTokenController), "GetAll")]
        [InlineData("/api/account/google-access-token/all", "HEAD", typeof(GoogleAccessTokenController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/export", "HEAD", typeof(GoogleAccessTokenController), "Export")]
        [InlineData("/api/account/google-access-token/export", "HEAD", typeof(GoogleAccessTokenController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/1", "HEAD", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/account/google-access-token/1", "HEAD", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/get?userIds=1", "HEAD", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/account/google-access-token/get?userIds=1", "HEAD", typeof(GoogleAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token", "HEAD", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/google-access-token", "HEAD", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/page/1", "HEAD", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/google-access-token/page/1", "HEAD", typeof(GoogleAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/count-filtered/{filterName}", "HEAD", typeof(GoogleAccessTokenController), "CountFiltered")]
        [InlineData("/api/account/google-access-token/count-filtered/{filterName}", "HEAD", typeof(GoogleAccessTokenController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(GoogleAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/google-access-token/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(GoogleAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/google-access-token/first", "HEAD", typeof(GoogleAccessTokenController), "GetFirst")]
        [InlineData("/api/account/google-access-token/previous/1", "HEAD", typeof(GoogleAccessTokenController), "GetPrevious")]
        [InlineData("/api/account/google-access-token/next/1", "HEAD", typeof(GoogleAccessTokenController), "GetNext")]
        [InlineData("/api/account/google-access-token/last", "HEAD", typeof(GoogleAccessTokenController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/google-access-token/custom-fields", "HEAD", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/google-access-token/custom-fields", "HEAD", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/google-access-token/custom-fields/{resourceId}", "HEAD", typeof(GoogleAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/google-access-token/custom-fields/{resourceId}", "HEAD", typeof(GoogleAccessTokenController), "GetCustomFields")]

        [Conditional("Debug")]
        public void TestRoute(string url, string verb, Type type, string actionName)
        {
            //Arrange
            url = url.Replace("{apiVersionNumber}", this.ApiVersionNumber);
            url = Host + url;

            //Act
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(verb), url);

            IHttpControllerSelector controller = this.GetControllerSelector();
            IHttpActionSelector action = this.GetActionSelector();

            IHttpRouteData route = this.Config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = route;
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = this.Config;

            HttpControllerDescriptor controllerDescriptor = controller.SelectController(request);

            HttpControllerContext context = new HttpControllerContext(this.Config, route, request)
            {
                ControllerDescriptor = controllerDescriptor
            };

            var actionDescriptor = action.SelectAction(context);

            //Assert
            Assert.NotNull(controllerDescriptor);
            Assert.NotNull(actionDescriptor);
            Assert.Equal(type, controllerDescriptor.ControllerType);
            Assert.Equal(actionName, actionDescriptor.ActionName);
        }

        #region Fixture
        private readonly HttpConfiguration Config;
        private readonly string Host;
        private readonly string ApiVersionNumber;

        public GoogleAccessTokenRouteTests()
        {
            this.Host = ConfigurationManager.AppSettings["HostPrefix"];
            this.ApiVersionNumber = ConfigurationManager.AppSettings["ApiVersionNumber"];
            this.Config = GetConfig();
        }

        private HttpConfiguration GetConfig()
        {
            if (MemoryCache.Default["Config"] == null)
            {
                HttpConfiguration config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute("VersionedApi", "api/" + this.ApiVersionNumber + "/{schema}/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
                config.Routes.MapHttpRoute("DefaultApi", "api/{schema}/{controller}/{action}/{id}", new { id = RouteParameter.Optional });

                config.EnsureInitialized();
                MemoryCache.Default["Config"] = config;
                return config;
            }

            return MemoryCache.Default["Config"] as HttpConfiguration;
        }

        private IHttpControllerSelector GetControllerSelector()
        {
            if (MemoryCache.Default["ControllerSelector"] == null)
            {
                IHttpControllerSelector selector = this.Config.Services.GetHttpControllerSelector();
                return selector;
            }

            return MemoryCache.Default["ControllerSelector"] as IHttpControllerSelector;
        }

        private IHttpActionSelector GetActionSelector()
        {
            if (MemoryCache.Default["ActionSelector"] == null)
            {
                IHttpActionSelector selector = this.Config.Services.GetActionSelector();
                return selector;
            }

            return MemoryCache.Default["ActionSelector"] as IHttpActionSelector;
        }
        #endregion
    }
}