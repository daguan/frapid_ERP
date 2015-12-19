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
    public class ResetRequestRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/delete/{requestId}", "DELETE", typeof(ResetRequestController), "Delete")]
        [InlineData("/api/account/reset-request/delete/{requestId}", "DELETE", typeof(ResetRequestController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/edit/{requestId}", "PUT", typeof(ResetRequestController), "Edit")]
        [InlineData("/api/account/reset-request/edit/{requestId}", "PUT", typeof(ResetRequestController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/count-where", "POST", typeof(ResetRequestController), "CountWhere")]
        [InlineData("/api/account/reset-request/count-where", "POST", typeof(ResetRequestController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/get-where/{pageNumber}", "POST", typeof(ResetRequestController), "GetWhere")]
        [InlineData("/api/account/reset-request/get-where/{pageNumber}", "POST", typeof(ResetRequestController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/add-or-edit", "POST", typeof(ResetRequestController), "AddOrEdit")]
        [InlineData("/api/account/reset-request/add-or-edit", "POST", typeof(ResetRequestController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/add/{resetRequest}", "POST", typeof(ResetRequestController), "Add")]
        [InlineData("/api/account/reset-request/add/{resetRequest}", "POST", typeof(ResetRequestController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/bulk-import", "POST", typeof(ResetRequestController), "BulkImport")]
        [InlineData("/api/account/reset-request/bulk-import", "POST", typeof(ResetRequestController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/meta", "GET", typeof(ResetRequestController), "GetEntityView")]
        [InlineData("/api/account/reset-request/meta", "GET", typeof(ResetRequestController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/count", "GET", typeof(ResetRequestController), "Count")]
        [InlineData("/api/account/reset-request/count", "GET", typeof(ResetRequestController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/all", "GET", typeof(ResetRequestController), "GetAll")]
        [InlineData("/api/account/reset-request/all", "GET", typeof(ResetRequestController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/export", "GET", typeof(ResetRequestController), "Export")]
        [InlineData("/api/account/reset-request/export", "GET", typeof(ResetRequestController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/1", "GET", typeof(ResetRequestController), "Get")]
        [InlineData("/api/account/reset-request/1", "GET", typeof(ResetRequestController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/get?requestIds=1", "GET", typeof(ResetRequestController), "Get")]
        [InlineData("/api/account/reset-request/get?requestIds=1", "GET", typeof(ResetRequestController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request", "GET", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/account/reset-request", "GET", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/page/1", "GET", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/account/reset-request/page/1", "GET", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/count-filtered/{filterName}", "GET", typeof(ResetRequestController), "CountFiltered")]
        [InlineData("/api/account/reset-request/count-filtered/{filterName}", "GET", typeof(ResetRequestController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ResetRequestController), "GetFiltered")]
        [InlineData("/api/account/reset-request/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ResetRequestController), "GetFiltered")]
        [InlineData("/api/account/reset-request/first", "GET", typeof(ResetRequestController), "GetFirst")]
        [InlineData("/api/account/reset-request/previous/1", "GET", typeof(ResetRequestController), "GetPrevious")]
        [InlineData("/api/account/reset-request/next/1", "GET", typeof(ResetRequestController), "GetNext")]
        [InlineData("/api/account/reset-request/last", "GET", typeof(ResetRequestController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/reset-request/custom-fields", "GET", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/account/reset-request/custom-fields", "GET", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/custom-fields/{resourceId}", "GET", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/account/reset-request/custom-fields/{resourceId}", "GET", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/meta", "HEAD", typeof(ResetRequestController), "GetEntityView")]
        [InlineData("/api/account/reset-request/meta", "HEAD", typeof(ResetRequestController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/count", "HEAD", typeof(ResetRequestController), "Count")]
        [InlineData("/api/account/reset-request/count", "HEAD", typeof(ResetRequestController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/all", "HEAD", typeof(ResetRequestController), "GetAll")]
        [InlineData("/api/account/reset-request/all", "HEAD", typeof(ResetRequestController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/export", "HEAD", typeof(ResetRequestController), "Export")]
        [InlineData("/api/account/reset-request/export", "HEAD", typeof(ResetRequestController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/1", "HEAD", typeof(ResetRequestController), "Get")]
        [InlineData("/api/account/reset-request/1", "HEAD", typeof(ResetRequestController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/get?requestIds=1", "HEAD", typeof(ResetRequestController), "Get")]
        [InlineData("/api/account/reset-request/get?requestIds=1", "HEAD", typeof(ResetRequestController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request", "HEAD", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/account/reset-request", "HEAD", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/page/1", "HEAD", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/account/reset-request/page/1", "HEAD", typeof(ResetRequestController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/count-filtered/{filterName}", "HEAD", typeof(ResetRequestController), "CountFiltered")]
        [InlineData("/api/account/reset-request/count-filtered/{filterName}", "HEAD", typeof(ResetRequestController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ResetRequestController), "GetFiltered")]
        [InlineData("/api/account/reset-request/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ResetRequestController), "GetFiltered")]
        [InlineData("/api/account/reset-request/first", "HEAD", typeof(ResetRequestController), "GetFirst")]
        [InlineData("/api/account/reset-request/previous/1", "HEAD", typeof(ResetRequestController), "GetPrevious")]
        [InlineData("/api/account/reset-request/next/1", "HEAD", typeof(ResetRequestController), "GetNext")]
        [InlineData("/api/account/reset-request/last", "HEAD", typeof(ResetRequestController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/reset-request/custom-fields", "HEAD", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/account/reset-request/custom-fields", "HEAD", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/reset-request/custom-fields/{resourceId}", "HEAD", typeof(ResetRequestController), "GetCustomFields")]
        [InlineData("/api/account/reset-request/custom-fields/{resourceId}", "HEAD", typeof(ResetRequestController), "GetCustomFields")]

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

        public ResetRequestRouteTests()
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