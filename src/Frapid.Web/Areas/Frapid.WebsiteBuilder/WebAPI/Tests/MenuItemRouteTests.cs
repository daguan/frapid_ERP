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

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class MenuItemRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/delete/{menuItemId}", "DELETE", typeof(MenuItemController), "Delete")]
        [InlineData("/api/website/menu-item/delete/{menuItemId}", "DELETE", typeof(MenuItemController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/edit/{menuItemId}", "PUT", typeof(MenuItemController), "Edit")]
        [InlineData("/api/website/menu-item/edit/{menuItemId}", "PUT", typeof(MenuItemController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/count-where", "POST", typeof(MenuItemController), "CountWhere")]
        [InlineData("/api/website/menu-item/count-where", "POST", typeof(MenuItemController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/get-where/{pageNumber}", "POST", typeof(MenuItemController), "GetWhere")]
        [InlineData("/api/website/menu-item/get-where/{pageNumber}", "POST", typeof(MenuItemController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/add-or-edit", "POST", typeof(MenuItemController), "AddOrEdit")]
        [InlineData("/api/website/menu-item/add-or-edit", "POST", typeof(MenuItemController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/add/{menuItem}", "POST", typeof(MenuItemController), "Add")]
        [InlineData("/api/website/menu-item/add/{menuItem}", "POST", typeof(MenuItemController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/bulk-import", "POST", typeof(MenuItemController), "BulkImport")]
        [InlineData("/api/website/menu-item/bulk-import", "POST", typeof(MenuItemController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/meta", "GET", typeof(MenuItemController), "GetEntityView")]
        [InlineData("/api/website/menu-item/meta", "GET", typeof(MenuItemController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/count", "GET", typeof(MenuItemController), "Count")]
        [InlineData("/api/website/menu-item/count", "GET", typeof(MenuItemController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/all", "GET", typeof(MenuItemController), "GetAll")]
        [InlineData("/api/website/menu-item/all", "GET", typeof(MenuItemController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/export", "GET", typeof(MenuItemController), "Export")]
        [InlineData("/api/website/menu-item/export", "GET", typeof(MenuItemController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/1", "GET", typeof(MenuItemController), "Get")]
        [InlineData("/api/website/menu-item/1", "GET", typeof(MenuItemController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/get?menuItemIds=1", "GET", typeof(MenuItemController), "Get")]
        [InlineData("/api/website/menu-item/get?menuItemIds=1", "GET", typeof(MenuItemController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item", "GET", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item", "GET", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/page/1", "GET", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item/page/1", "GET", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/count-filtered/{filterName}", "GET", typeof(MenuItemController), "CountFiltered")]
        [InlineData("/api/website/menu-item/count-filtered/{filterName}", "GET", typeof(MenuItemController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuItemController), "GetFiltered")]
        [InlineData("/api/website/menu-item/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuItemController), "GetFiltered")]
        [InlineData("/api/website/menu-item/first", "GET", typeof(MenuItemController), "GetFirst")]
        [InlineData("/api/website/menu-item/previous/1", "GET", typeof(MenuItemController), "GetPrevious")]
        [InlineData("/api/website/menu-item/next/1", "GET", typeof(MenuItemController), "GetNext")]
        [InlineData("/api/website/menu-item/last", "GET", typeof(MenuItemController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/menu-item/custom-fields", "GET", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/website/menu-item/custom-fields", "GET", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/custom-fields/{resourceId}", "GET", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/website/menu-item/custom-fields/{resourceId}", "GET", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/meta", "HEAD", typeof(MenuItemController), "GetEntityView")]
        [InlineData("/api/website/menu-item/meta", "HEAD", typeof(MenuItemController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/count", "HEAD", typeof(MenuItemController), "Count")]
        [InlineData("/api/website/menu-item/count", "HEAD", typeof(MenuItemController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/all", "HEAD", typeof(MenuItemController), "GetAll")]
        [InlineData("/api/website/menu-item/all", "HEAD", typeof(MenuItemController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/export", "HEAD", typeof(MenuItemController), "Export")]
        [InlineData("/api/website/menu-item/export", "HEAD", typeof(MenuItemController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/1", "HEAD", typeof(MenuItemController), "Get")]
        [InlineData("/api/website/menu-item/1", "HEAD", typeof(MenuItemController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/get?menuItemIds=1", "HEAD", typeof(MenuItemController), "Get")]
        [InlineData("/api/website/menu-item/get?menuItemIds=1", "HEAD", typeof(MenuItemController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item", "HEAD", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item", "HEAD", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/page/1", "HEAD", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item/page/1", "HEAD", typeof(MenuItemController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/count-filtered/{filterName}", "HEAD", typeof(MenuItemController), "CountFiltered")]
        [InlineData("/api/website/menu-item/count-filtered/{filterName}", "HEAD", typeof(MenuItemController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuItemController), "GetFiltered")]
        [InlineData("/api/website/menu-item/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuItemController), "GetFiltered")]
        [InlineData("/api/website/menu-item/first", "HEAD", typeof(MenuItemController), "GetFirst")]
        [InlineData("/api/website/menu-item/previous/1", "HEAD", typeof(MenuItemController), "GetPrevious")]
        [InlineData("/api/website/menu-item/next/1", "HEAD", typeof(MenuItemController), "GetNext")]
        [InlineData("/api/website/menu-item/last", "HEAD", typeof(MenuItemController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/menu-item/custom-fields", "HEAD", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/website/menu-item/custom-fields", "HEAD", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item/custom-fields/{resourceId}", "HEAD", typeof(MenuItemController), "GetCustomFields")]
        [InlineData("/api/website/menu-item/custom-fields/{resourceId}", "HEAD", typeof(MenuItemController), "GetCustomFields")]

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

        public MenuItemRouteTests()
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