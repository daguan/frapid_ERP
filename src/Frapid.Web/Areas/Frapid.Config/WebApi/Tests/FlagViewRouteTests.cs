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

namespace Frapid.Config.Api.Tests
{
    public class FlagViewRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/count", "GET", typeof(FlagViewController), "Count")]
        [InlineData("/api/config/flag-view/count", "GET", typeof(FlagViewController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/all", "GET", typeof(FlagViewController), "Get")]
        [InlineData("/api/config/flag-view/all", "GET", typeof(FlagViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/export", "GET", typeof(FlagViewController), "Get")]
        [InlineData("/api/config/flag-view/export", "GET", typeof(FlagViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view", "GET", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-view", "GET", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/page/1", "GET", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-view/page/1", "GET", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/count-filtered/{filterName}", "GET", typeof(FlagViewController), "CountFiltered")]
        [InlineData("/api/config/flag-view/count-filtered/{filterName}", "GET", typeof(FlagViewController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagViewController), "GetFiltered")]
        [InlineData("/api/config/flag-view/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagViewController), "GetFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/display-fields", "GET", typeof(FlagViewController), "GetDisplayFields")]
        [InlineData("/api/config/flag-view/display-fields", "GET", typeof(FlagViewController), "GetDisplayFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/count", "HEAD", typeof(FlagViewController), "Count")]
        [InlineData("/api/config/flag-view/count", "HEAD", typeof(FlagViewController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/all", "HEAD", typeof(FlagViewController), "Get")]
        [InlineData("/api/config/flag-view/all", "HEAD", typeof(FlagViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/export", "HEAD", typeof(FlagViewController), "Get")]
        [InlineData("/api/config/flag-view/export", "HEAD", typeof(FlagViewController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view", "HEAD", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-view", "HEAD", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/page/1", "HEAD", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-view/page/1", "HEAD", typeof(FlagViewController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/count-filtered/{filterName}", "HEAD", typeof(FlagViewController), "CountFiltered")]
        [InlineData("/api/config/flag-view/count-filtered/{filterName}", "HEAD", typeof(FlagViewController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagViewController), "GetFiltered")]
        [InlineData("/api/config/flag-view/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagViewController), "GetFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-view/display-fields", "HEAD", typeof(FlagViewController), "GetDisplayFields")]
        [InlineData("/api/config/flag-view/display-fields", "HEAD", typeof(FlagViewController), "GetDisplayFields")]

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

        public FlagViewRouteTests()
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