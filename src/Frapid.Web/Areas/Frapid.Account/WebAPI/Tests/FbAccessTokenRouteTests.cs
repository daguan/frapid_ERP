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
    public class FbAccessTokenRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/delete/{userId}", "DELETE", typeof(FbAccessTokenController), "Delete")]
        [InlineData("/api/account/fb-access-token/delete/{userId}", "DELETE", typeof(FbAccessTokenController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/edit/{userId}", "PUT", typeof(FbAccessTokenController), "Edit")]
        [InlineData("/api/account/fb-access-token/edit/{userId}", "PUT", typeof(FbAccessTokenController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/count-where", "POST", typeof(FbAccessTokenController), "CountWhere")]
        [InlineData("/api/account/fb-access-token/count-where", "POST", typeof(FbAccessTokenController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/get-where/{pageNumber}", "POST", typeof(FbAccessTokenController), "GetWhere")]
        [InlineData("/api/account/fb-access-token/get-where/{pageNumber}", "POST", typeof(FbAccessTokenController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/add-or-edit", "POST", typeof(FbAccessTokenController), "AddOrEdit")]
        [InlineData("/api/account/fb-access-token/add-or-edit", "POST", typeof(FbAccessTokenController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/add/{fbAccessToken}", "POST", typeof(FbAccessTokenController), "Add")]
        [InlineData("/api/account/fb-access-token/add/{fbAccessToken}", "POST", typeof(FbAccessTokenController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/bulk-import", "POST", typeof(FbAccessTokenController), "BulkImport")]
        [InlineData("/api/account/fb-access-token/bulk-import", "POST", typeof(FbAccessTokenController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/meta", "GET", typeof(FbAccessTokenController), "GetEntityView")]
        [InlineData("/api/account/fb-access-token/meta", "GET", typeof(FbAccessTokenController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/count", "GET", typeof(FbAccessTokenController), "Count")]
        [InlineData("/api/account/fb-access-token/count", "GET", typeof(FbAccessTokenController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/all", "GET", typeof(FbAccessTokenController), "GetAll")]
        [InlineData("/api/account/fb-access-token/all", "GET", typeof(FbAccessTokenController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/export", "GET", typeof(FbAccessTokenController), "Export")]
        [InlineData("/api/account/fb-access-token/export", "GET", typeof(FbAccessTokenController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/1", "GET", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/account/fb-access-token/1", "GET", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/get?userIds=1", "GET", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/account/fb-access-token/get?userIds=1", "GET", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token", "GET", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/fb-access-token", "GET", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/page/1", "GET", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/fb-access-token/page/1", "GET", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/count-filtered/{filterName}", "GET", typeof(FbAccessTokenController), "CountFiltered")]
        [InlineData("/api/account/fb-access-token/count-filtered/{filterName}", "GET", typeof(FbAccessTokenController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FbAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/fb-access-token/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FbAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/fb-access-token/first", "GET", typeof(FbAccessTokenController), "GetFirst")]
        [InlineData("/api/account/fb-access-token/previous/1", "GET", typeof(FbAccessTokenController), "GetPrevious")]
        [InlineData("/api/account/fb-access-token/next/1", "GET", typeof(FbAccessTokenController), "GetNext")]
        [InlineData("/api/account/fb-access-token/last", "GET", typeof(FbAccessTokenController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/custom-fields", "GET", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/fb-access-token/custom-fields", "GET", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/custom-fields/{resourceId}", "GET", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/fb-access-token/custom-fields/{resourceId}", "GET", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/meta", "HEAD", typeof(FbAccessTokenController), "GetEntityView")]
        [InlineData("/api/account/fb-access-token/meta", "HEAD", typeof(FbAccessTokenController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/count", "HEAD", typeof(FbAccessTokenController), "Count")]
        [InlineData("/api/account/fb-access-token/count", "HEAD", typeof(FbAccessTokenController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/all", "HEAD", typeof(FbAccessTokenController), "GetAll")]
        [InlineData("/api/account/fb-access-token/all", "HEAD", typeof(FbAccessTokenController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/export", "HEAD", typeof(FbAccessTokenController), "Export")]
        [InlineData("/api/account/fb-access-token/export", "HEAD", typeof(FbAccessTokenController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/1", "HEAD", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/account/fb-access-token/1", "HEAD", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/get?userIds=1", "HEAD", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/account/fb-access-token/get?userIds=1", "HEAD", typeof(FbAccessTokenController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token", "HEAD", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/fb-access-token", "HEAD", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/page/1", "HEAD", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/account/fb-access-token/page/1", "HEAD", typeof(FbAccessTokenController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/count-filtered/{filterName}", "HEAD", typeof(FbAccessTokenController), "CountFiltered")]
        [InlineData("/api/account/fb-access-token/count-filtered/{filterName}", "HEAD", typeof(FbAccessTokenController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FbAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/fb-access-token/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FbAccessTokenController), "GetFiltered")]
        [InlineData("/api/account/fb-access-token/first", "HEAD", typeof(FbAccessTokenController), "GetFirst")]
        [InlineData("/api/account/fb-access-token/previous/1", "HEAD", typeof(FbAccessTokenController), "GetPrevious")]
        [InlineData("/api/account/fb-access-token/next/1", "HEAD", typeof(FbAccessTokenController), "GetNext")]
        [InlineData("/api/account/fb-access-token/last", "HEAD", typeof(FbAccessTokenController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/custom-fields", "HEAD", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/fb-access-token/custom-fields", "HEAD", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/fb-access-token/custom-fields/{resourceId}", "HEAD", typeof(FbAccessTokenController), "GetCustomFields")]
        [InlineData("/api/account/fb-access-token/custom-fields/{resourceId}", "HEAD", typeof(FbAccessTokenController), "GetCustomFields")]

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

        public FbAccessTokenRouteTests()
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