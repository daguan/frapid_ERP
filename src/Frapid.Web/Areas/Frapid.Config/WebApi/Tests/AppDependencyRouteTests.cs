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
    public class AppDependencyRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/delete/{appDependencyId}", "DELETE", typeof(AppDependencyController), "Delete")]
        [InlineData("/api/config/app-dependency/delete/{appDependencyId}", "DELETE", typeof(AppDependencyController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/edit/{appDependencyId}", "PUT", typeof(AppDependencyController), "Edit")]
        [InlineData("/api/config/app-dependency/edit/{appDependencyId}", "PUT", typeof(AppDependencyController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/count-where", "POST", typeof(AppDependencyController), "CountWhere")]
        [InlineData("/api/config/app-dependency/count-where", "POST", typeof(AppDependencyController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/get-where/{pageNumber}", "POST", typeof(AppDependencyController), "GetWhere")]
        [InlineData("/api/config/app-dependency/get-where/{pageNumber}", "POST", typeof(AppDependencyController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/add-or-edit", "POST", typeof(AppDependencyController), "AddOrEdit")]
        [InlineData("/api/config/app-dependency/add-or-edit", "POST", typeof(AppDependencyController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/add/{appDependency}", "POST", typeof(AppDependencyController), "Add")]
        [InlineData("/api/config/app-dependency/add/{appDependency}", "POST", typeof(AppDependencyController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/bulk-import", "POST", typeof(AppDependencyController), "BulkImport")]
        [InlineData("/api/config/app-dependency/bulk-import", "POST", typeof(AppDependencyController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/meta", "GET", typeof(AppDependencyController), "GetEntityView")]
        [InlineData("/api/config/app-dependency/meta", "GET", typeof(AppDependencyController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/count", "GET", typeof(AppDependencyController), "Count")]
        [InlineData("/api/config/app-dependency/count", "GET", typeof(AppDependencyController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/all", "GET", typeof(AppDependencyController), "GetAll")]
        [InlineData("/api/config/app-dependency/all", "GET", typeof(AppDependencyController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/export", "GET", typeof(AppDependencyController), "Export")]
        [InlineData("/api/config/app-dependency/export", "GET", typeof(AppDependencyController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/1", "GET", typeof(AppDependencyController), "Get")]
        [InlineData("/api/config/app-dependency/1", "GET", typeof(AppDependencyController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/get?appDependencyIds=1", "GET", typeof(AppDependencyController), "Get")]
        [InlineData("/api/config/app-dependency/get?appDependencyIds=1", "GET", typeof(AppDependencyController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency", "GET", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/config/app-dependency", "GET", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/page/1", "GET", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/config/app-dependency/page/1", "GET", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/count-filtered/{filterName}", "GET", typeof(AppDependencyController), "CountFiltered")]
        [InlineData("/api/config/app-dependency/count-filtered/{filterName}", "GET", typeof(AppDependencyController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AppDependencyController), "GetFiltered")]
        [InlineData("/api/config/app-dependency/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AppDependencyController), "GetFiltered")]
        [InlineData("/api/config/app-dependency/first", "GET", typeof(AppDependencyController), "GetFirst")]
        [InlineData("/api/config/app-dependency/previous/1", "GET", typeof(AppDependencyController), "GetPrevious")]
        [InlineData("/api/config/app-dependency/next/1", "GET", typeof(AppDependencyController), "GetNext")]
        [InlineData("/api/config/app-dependency/last", "GET", typeof(AppDependencyController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/app-dependency/custom-fields", "GET", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/config/app-dependency/custom-fields", "GET", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/custom-fields/{resourceId}", "GET", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/config/app-dependency/custom-fields/{resourceId}", "GET", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/meta", "HEAD", typeof(AppDependencyController), "GetEntityView")]
        [InlineData("/api/config/app-dependency/meta", "HEAD", typeof(AppDependencyController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/count", "HEAD", typeof(AppDependencyController), "Count")]
        [InlineData("/api/config/app-dependency/count", "HEAD", typeof(AppDependencyController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/all", "HEAD", typeof(AppDependencyController), "GetAll")]
        [InlineData("/api/config/app-dependency/all", "HEAD", typeof(AppDependencyController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/export", "HEAD", typeof(AppDependencyController), "Export")]
        [InlineData("/api/config/app-dependency/export", "HEAD", typeof(AppDependencyController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/1", "HEAD", typeof(AppDependencyController), "Get")]
        [InlineData("/api/config/app-dependency/1", "HEAD", typeof(AppDependencyController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/get?appDependencyIds=1", "HEAD", typeof(AppDependencyController), "Get")]
        [InlineData("/api/config/app-dependency/get?appDependencyIds=1", "HEAD", typeof(AppDependencyController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency", "HEAD", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/config/app-dependency", "HEAD", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/page/1", "HEAD", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/config/app-dependency/page/1", "HEAD", typeof(AppDependencyController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/count-filtered/{filterName}", "HEAD", typeof(AppDependencyController), "CountFiltered")]
        [InlineData("/api/config/app-dependency/count-filtered/{filterName}", "HEAD", typeof(AppDependencyController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AppDependencyController), "GetFiltered")]
        [InlineData("/api/config/app-dependency/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AppDependencyController), "GetFiltered")]
        [InlineData("/api/config/app-dependency/first", "HEAD", typeof(AppDependencyController), "GetFirst")]
        [InlineData("/api/config/app-dependency/previous/1", "HEAD", typeof(AppDependencyController), "GetPrevious")]
        [InlineData("/api/config/app-dependency/next/1", "HEAD", typeof(AppDependencyController), "GetNext")]
        [InlineData("/api/config/app-dependency/last", "HEAD", typeof(AppDependencyController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/app-dependency/custom-fields", "HEAD", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/config/app-dependency/custom-fields", "HEAD", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/app-dependency/custom-fields/{resourceId}", "HEAD", typeof(AppDependencyController), "GetCustomFields")]
        [InlineData("/api/config/app-dependency/custom-fields/{resourceId}", "HEAD", typeof(AppDependencyController), "GetCustomFields")]

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

        public AppDependencyRouteTests()
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