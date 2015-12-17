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
    public class MenuRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/menu/delete/{menuId}", "DELETE", typeof(ConfigMenuController), "Delete")]
        [InlineData("/api/config/menu/delete/{menuId}", "DELETE", typeof(ConfigMenuController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/menu/edit/{menuId}", "PUT", typeof(ConfigMenuController), "Edit")]
        [InlineData("/api/config/menu/edit/{menuId}", "PUT", typeof(ConfigMenuController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-where", "POST", typeof(ConfigMenuController), "CountWhere")]
        [InlineData("/api/config/menu/count-where", "POST", typeof(ConfigMenuController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-where/{pageNumber}", "POST", typeof(ConfigMenuController), "GetWhere")]
        [InlineData("/api/config/menu/get-where/{pageNumber}", "POST", typeof(ConfigMenuController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/menu/add-or-edit", "POST", typeof(ConfigMenuController), "AddOrEdit")]
        [InlineData("/api/config/menu/add-or-edit", "POST", typeof(ConfigMenuController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/menu/add/{menu}", "POST", typeof(ConfigMenuController), "Add")]
        [InlineData("/api/config/menu/add/{menu}", "POST", typeof(ConfigMenuController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/menu/bulk-import", "POST", typeof(ConfigMenuController), "BulkImport")]
        [InlineData("/api/config/menu/bulk-import", "POST", typeof(ConfigMenuController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/menu/meta", "GET", typeof(ConfigMenuController), "GetEntityView")]
        [InlineData("/api/config/menu/meta", "GET", typeof(ConfigMenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count", "GET", typeof(ConfigMenuController), "Count")]
        [InlineData("/api/config/menu/count", "GET", typeof(ConfigMenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/menu/all", "GET", typeof(ConfigMenuController), "GetAll")]
        [InlineData("/api/config/menu/all", "GET", typeof(ConfigMenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/menu/export", "GET", typeof(ConfigMenuController), "Export")]
        [InlineData("/api/config/menu/export", "GET", typeof(ConfigMenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/menu/1", "GET", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/config/menu/1", "GET", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get?menuIds=1", "GET", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/config/menu/get?menuIds=1", "GET", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu", "GET", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu", "GET", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/page/1", "GET", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu/page/1", "GET", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-filtered/{filterName}", "GET", typeof(ConfigMenuController), "CountFiltered")]
        [InlineData("/api/config/menu/count-filtered/{filterName}", "GET", typeof(ConfigMenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ConfigMenuController), "GetFiltered")]
        [InlineData("/api/config/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ConfigMenuController), "GetFiltered")]
        [InlineData("/api/config/menu/first", "GET", typeof(ConfigMenuController), "GetFirst")]
        [InlineData("/api/config/menu/previous/1", "GET", typeof(ConfigMenuController), "GetPrevious")]
        [InlineData("/api/config/menu/next/1", "GET", typeof(ConfigMenuController), "GetNext")]
        [InlineData("/api/config/menu/last", "GET", typeof(ConfigMenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields", "GET", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields", "GET", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields/{resourceId}", "GET", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields/{resourceId}", "GET", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/meta", "HEAD", typeof(ConfigMenuController), "GetEntityView")]
        [InlineData("/api/config/menu/meta", "HEAD", typeof(ConfigMenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count", "HEAD", typeof(ConfigMenuController), "Count")]
        [InlineData("/api/config/menu/count", "HEAD", typeof(ConfigMenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/menu/all", "HEAD", typeof(ConfigMenuController), "GetAll")]
        [InlineData("/api/config/menu/all", "HEAD", typeof(ConfigMenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/menu/export", "HEAD", typeof(ConfigMenuController), "Export")]
        [InlineData("/api/config/menu/export", "HEAD", typeof(ConfigMenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/menu/1", "HEAD", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/config/menu/1", "HEAD", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get?menuIds=1", "HEAD", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/config/menu/get?menuIds=1", "HEAD", typeof(ConfigMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu", "HEAD", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu", "HEAD", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/page/1", "HEAD", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu/page/1", "HEAD", typeof(ConfigMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-filtered/{filterName}", "HEAD", typeof(ConfigMenuController), "CountFiltered")]
        [InlineData("/api/config/menu/count-filtered/{filterName}", "HEAD", typeof(ConfigMenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ConfigMenuController), "GetFiltered")]
        [InlineData("/api/config/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ConfigMenuController), "GetFiltered")]
        [InlineData("/api/config/menu/first", "HEAD", typeof(ConfigMenuController), "GetFirst")]
        [InlineData("/api/config/menu/previous/1", "HEAD", typeof(ConfigMenuController), "GetPrevious")]
        [InlineData("/api/config/menu/next/1", "HEAD", typeof(ConfigMenuController), "GetNext")]
        [InlineData("/api/config/menu/last", "HEAD", typeof(ConfigMenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields", "HEAD", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields", "HEAD", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields/{resourceId}", "HEAD", typeof(ConfigMenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields/{resourceId}", "HEAD", typeof(ConfigMenuController), "GetCustomFields")]

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

        public MenuRouteTests()
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