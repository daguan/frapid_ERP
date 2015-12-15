// ReSharper disable All
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class MenuItemViewRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/count", "GET", typeof(MenuItemViewController), "Count")]
        [InlineData("/api/website/menu-item-view/count", "GET", typeof(MenuItemViewController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/all", "GET", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/website/menu-item-view/all", "GET", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/export", "GET", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/website/menu-item-view/export", "GET", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view", "GET", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item-view", "GET", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/page/1", "GET", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item-view/page/1", "GET", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/count-filtered/{filterName}", "GET", typeof(MenuItemViewController), "CountFiltered")]
        [InlineData("/api/website/menu-item-view/count-filtered/{filterName}", "GET", typeof(MenuItemViewController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuItemViewController), "GetFiltered")]
        [InlineData("/api/website/menu-item-view/get-filtered/{pageNumber}/{filterName}", "GET", typeof(MenuItemViewController), "GetFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/display-fields", "GET", typeof(MenuItemViewController), "GetDisplayFields")]
        [InlineData("/api/website/menu-item-view/display-fields", "GET", typeof(MenuItemViewController), "GetDisplayFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/count", "HEAD", typeof(MenuItemViewController), "Count")]
        [InlineData("/api/website/menu-item-view/count", "HEAD", typeof(MenuItemViewController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/all", "HEAD", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/website/menu-item-view/all", "HEAD", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/export", "HEAD", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/website/menu-item-view/export", "HEAD", typeof(MenuItemViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view", "HEAD", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item-view", "HEAD", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/page/1", "HEAD", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/website/menu-item-view/page/1", "HEAD", typeof(MenuItemViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/count-filtered/{filterName}", "HEAD", typeof(MenuItemViewController), "CountFiltered")]
        [InlineData("/api/website/menu-item-view/count-filtered/{filterName}", "HEAD", typeof(MenuItemViewController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuItemViewController), "GetFiltered")]
        [InlineData("/api/website/menu-item-view/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(MenuItemViewController), "GetFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu-item-view/display-fields", "HEAD", typeof(MenuItemViewController), "GetDisplayFields")]
        [InlineData("/api/website/menu-item-view/display-fields", "HEAD", typeof(MenuItemViewController), "GetDisplayFields")]

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

        public MenuItemViewRouteTests()
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