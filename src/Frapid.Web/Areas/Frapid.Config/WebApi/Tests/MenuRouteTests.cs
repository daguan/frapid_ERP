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
        [InlineData("/api/{apiVersionNumber}/config/menu/delete/{menuId}", "DELETE", typeof(MenuController), "Delete")]
        [InlineData("/api/config/menu/delete/{menuId}", "DELETE", typeof(MenuController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/menu/edit/{menuId}", "PUT", typeof(MenuController), "Edit")]
        [InlineData("/api/config/menu/edit/{menuId}", "PUT", typeof(MenuController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-where", "POST", typeof(MenuController), "CountWhere")]
        [InlineData("/api/config/menu/count-where", "POST", typeof(MenuController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-where/{pageNumber}", "POST", typeof(MenuController), "GetWhere")]
        [InlineData("/api/config/menu/get-where/{pageNumber}", "POST", typeof(MenuController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/menu/add-or-edit", "POST", typeof(MenuController), "AddOrEdit")]
        [InlineData("/api/config/menu/add-or-edit", "POST", typeof(MenuController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/menu/add/{menu}", "POST", typeof(MenuController), "Add")]
        [InlineData("/api/config/menu/add/{menu}", "POST", typeof(MenuController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/menu/bulk-import", "POST", typeof(MenuController), "BulkImport")]
        [InlineData("/api/config/menu/bulk-import", "POST", typeof(MenuController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/menu/meta", "GET", typeof(MenuController), "GetEntityView")]
        [InlineData("/api/config/menu/meta", "GET", typeof(MenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count", "GET", typeof(MenuController), "Count")]
        [InlineData("/api/config/menu/count", "GET", typeof(MenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/menu/all", "GET", typeof(MenuController), "GetAll")]
        [InlineData("/api/config/menu/all", "GET", typeof(MenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/menu/export", "GET", typeof(MenuController), "Export")]
        [InlineData("/api/config/menu/export", "GET", typeof(MenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/menu/1", "GET", typeof(MenuController), "Get")]
        [InlineData("/api/config/menu/1", "GET", typeof(MenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get?menuIds=1", "GET", typeof(MenuController), "Get")]
        [InlineData("/api/config/menu/get?menuIds=1", "GET", typeof(MenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu", "GET", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu", "GET", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/page/1", "GET", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu/page/1", "GET", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-filtered/{filterName}", "GET", typeof(MenuController), "CountFiltered")]
        [InlineData("/api/config/menu/count-filtered/{filterName}", "GET", typeof(MenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuController), "GetFiltered")]
        [InlineData("/api/config/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuController), "GetFiltered")]
        [InlineData("/api/config/menu/first", "GET", typeof(MenuController), "GetFirst")]
        [InlineData("/api/config/menu/previous/1", "GET", typeof(MenuController), "GetPrevious")]
        [InlineData("/api/config/menu/next/1", "GET", typeof(MenuController), "GetNext")]
        [InlineData("/api/config/menu/last", "GET", typeof(MenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields", "GET", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields", "GET", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields/{resourceId}", "GET", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields/{resourceId}", "GET", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/meta", "HEAD", typeof(MenuController), "GetEntityView")]
        [InlineData("/api/config/menu/meta", "HEAD", typeof(MenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count", "HEAD", typeof(MenuController), "Count")]
        [InlineData("/api/config/menu/count", "HEAD", typeof(MenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/menu/all", "HEAD", typeof(MenuController), "GetAll")]
        [InlineData("/api/config/menu/all", "HEAD", typeof(MenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/menu/export", "HEAD", typeof(MenuController), "Export")]
        [InlineData("/api/config/menu/export", "HEAD", typeof(MenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/menu/1", "HEAD", typeof(MenuController), "Get")]
        [InlineData("/api/config/menu/1", "HEAD", typeof(MenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get?menuIds=1", "HEAD", typeof(MenuController), "Get")]
        [InlineData("/api/config/menu/get?menuIds=1", "HEAD", typeof(MenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/menu", "HEAD", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu", "HEAD", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/page/1", "HEAD", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/config/menu/page/1", "HEAD", typeof(MenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/menu/count-filtered/{filterName}", "HEAD", typeof(MenuController), "CountFiltered")]
        [InlineData("/api/config/menu/count-filtered/{filterName}", "HEAD", typeof(MenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuController), "GetFiltered")]
        [InlineData("/api/config/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuController), "GetFiltered")]
        [InlineData("/api/config/menu/first", "HEAD", typeof(MenuController), "GetFirst")]
        [InlineData("/api/config/menu/previous/1", "HEAD", typeof(MenuController), "GetPrevious")]
        [InlineData("/api/config/menu/next/1", "HEAD", typeof(MenuController), "GetNext")]
        [InlineData("/api/config/menu/last", "HEAD", typeof(MenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields", "HEAD", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields", "HEAD", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/menu/custom-fields/{resourceId}", "HEAD", typeof(MenuController), "GetCustomFields")]
        [InlineData("/api/config/menu/custom-fields/{resourceId}", "HEAD", typeof(MenuController), "GetCustomFields")]

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