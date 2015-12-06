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

namespace Frapid.Config.Api.Tests
{
    public class AppRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/app/delete/{appName}", "DELETE", typeof(AppController), "Delete")]
        [InlineData("/api/config/app/delete/{appName}", "DELETE", typeof(AppController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/app/edit/{appName}", "PUT", typeof(AppController), "Edit")]
        [InlineData("/api/config/app/edit/{appName}", "PUT", typeof(AppController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/app/count-where", "POST", typeof(AppController), "CountWhere")]
        [InlineData("/api/config/app/count-where", "POST", typeof(AppController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/app/get-where/{pageNumber}", "POST", typeof(AppController), "GetWhere")]
        [InlineData("/api/config/app/get-where/{pageNumber}", "POST", typeof(AppController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/app/add-or-edit", "POST", typeof(AppController), "AddOrEdit")]
        [InlineData("/api/config/app/add-or-edit", "POST", typeof(AppController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/app/add/{app}", "POST", typeof(AppController), "Add")]
        [InlineData("/api/config/app/add/{app}", "POST", typeof(AppController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/app/bulk-import", "POST", typeof(AppController), "BulkImport")]
        [InlineData("/api/config/app/bulk-import", "POST", typeof(AppController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/app/meta", "GET", typeof(AppController), "GetEntityView")]
        [InlineData("/api/config/app/meta", "GET", typeof(AppController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/app/count", "GET", typeof(AppController), "Count")]
        [InlineData("/api/config/app/count", "GET", typeof(AppController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/app/all", "GET", typeof(AppController), "GetAll")]
        [InlineData("/api/config/app/all", "GET", typeof(AppController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/app/export", "GET", typeof(AppController), "Export")]
        [InlineData("/api/config/app/export", "GET", typeof(AppController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/app/1", "GET", typeof(AppController), "Get")]
        [InlineData("/api/config/app/1", "GET", typeof(AppController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app/get?appNames=1", "GET", typeof(AppController), "Get")]
        [InlineData("/api/config/app/get?appNames=1", "GET", typeof(AppController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app", "GET", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/config/app", "GET", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app/page/1", "GET", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/config/app/page/1", "GET", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app/count-filtered/{filterName}", "GET", typeof(AppController), "CountFiltered")]
        [InlineData("/api/config/app/count-filtered/{filterName}", "GET", typeof(AppController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/app/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AppController), "GetFiltered")]
        [InlineData("/api/config/app/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AppController), "GetFiltered")]
        [InlineData("/api/config/app/first", "GET", typeof(AppController), "GetFirst")]
        [InlineData("/api/config/app/previous/1", "GET", typeof(AppController), "GetPrevious")]
        [InlineData("/api/config/app/next/1", "GET", typeof(AppController), "GetNext")]
        [InlineData("/api/config/app/last", "GET", typeof(AppController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/app/custom-fields", "GET", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/config/app/custom-fields", "GET", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app/custom-fields/{resourceId}", "GET", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/config/app/custom-fields/{resourceId}", "GET", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app/meta", "HEAD", typeof(AppController), "GetEntityView")]
        [InlineData("/api/config/app/meta", "HEAD", typeof(AppController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/app/count", "HEAD", typeof(AppController), "Count")]
        [InlineData("/api/config/app/count", "HEAD", typeof(AppController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/app/all", "HEAD", typeof(AppController), "GetAll")]
        [InlineData("/api/config/app/all", "HEAD", typeof(AppController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/app/export", "HEAD", typeof(AppController), "Export")]
        [InlineData("/api/config/app/export", "HEAD", typeof(AppController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/app/1", "HEAD", typeof(AppController), "Get")]
        [InlineData("/api/config/app/1", "HEAD", typeof(AppController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app/get?appNames=1", "HEAD", typeof(AppController), "Get")]
        [InlineData("/api/config/app/get?appNames=1", "HEAD", typeof(AppController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app", "HEAD", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/config/app", "HEAD", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app/page/1", "HEAD", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/config/app/page/1", "HEAD", typeof(AppController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app/count-filtered/{filterName}", "HEAD", typeof(AppController), "CountFiltered")]
        [InlineData("/api/config/app/count-filtered/{filterName}", "HEAD", typeof(AppController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/app/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AppController), "GetFiltered")]
        [InlineData("/api/config/app/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AppController), "GetFiltered")]
        [InlineData("/api/config/app/first", "HEAD", typeof(AppController), "GetFirst")]
        [InlineData("/api/config/app/previous/1", "HEAD", typeof(AppController), "GetPrevious")]
        [InlineData("/api/config/app/next/1", "HEAD", typeof(AppController), "GetNext")]
        [InlineData("/api/config/app/last", "HEAD", typeof(AppController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/app/custom-fields", "HEAD", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/config/app/custom-fields", "HEAD", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app/custom-fields/{resourceId}", "HEAD", typeof(AppController), "GetCustomFields")]
        [InlineData("/api/config/app/custom-fields/{resourceId}", "HEAD", typeof(AppController), "GetCustomFields")]

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

        public AppRouteTests()
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