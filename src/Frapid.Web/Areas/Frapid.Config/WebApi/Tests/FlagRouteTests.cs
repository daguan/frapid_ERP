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
    public class FlagRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/flag/delete/{flagId}", "DELETE", typeof(FlagController), "Delete")]
        [InlineData("/api/config/flag/delete/{flagId}", "DELETE", typeof(FlagController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/flag/edit/{flagId}", "PUT", typeof(FlagController), "Edit")]
        [InlineData("/api/config/flag/edit/{flagId}", "PUT", typeof(FlagController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/flag/count-where", "POST", typeof(FlagController), "CountWhere")]
        [InlineData("/api/config/flag/count-where", "POST", typeof(FlagController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/flag/get-where/{pageNumber}", "POST", typeof(FlagController), "GetWhere")]
        [InlineData("/api/config/flag/get-where/{pageNumber}", "POST", typeof(FlagController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/flag/add-or-edit", "POST", typeof(FlagController), "AddOrEdit")]
        [InlineData("/api/config/flag/add-or-edit", "POST", typeof(FlagController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/flag/add/{flag}", "POST", typeof(FlagController), "Add")]
        [InlineData("/api/config/flag/add/{flag}", "POST", typeof(FlagController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/flag/bulk-import", "POST", typeof(FlagController), "BulkImport")]
        [InlineData("/api/config/flag/bulk-import", "POST", typeof(FlagController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/flag/meta", "GET", typeof(FlagController), "GetEntityView")]
        [InlineData("/api/config/flag/meta", "GET", typeof(FlagController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/flag/count", "GET", typeof(FlagController), "Count")]
        [InlineData("/api/config/flag/count", "GET", typeof(FlagController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag/all", "GET", typeof(FlagController), "GetAll")]
        [InlineData("/api/config/flag/all", "GET", typeof(FlagController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/flag/export", "GET", typeof(FlagController), "Export")]
        [InlineData("/api/config/flag/export", "GET", typeof(FlagController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/flag/1", "GET", typeof(FlagController), "Get")]
        [InlineData("/api/config/flag/1", "GET", typeof(FlagController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag/get?flagIds=1", "GET", typeof(FlagController), "Get")]
        [InlineData("/api/config/flag/get?flagIds=1", "GET", typeof(FlagController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag", "GET", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/config/flag", "GET", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag/page/1", "GET", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/config/flag/page/1", "GET", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag/count-filtered/{filterName}", "GET", typeof(FlagController), "CountFiltered")]
        [InlineData("/api/config/flag/count-filtered/{filterName}", "GET", typeof(FlagController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagController), "GetFiltered")]
        [InlineData("/api/config/flag/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagController), "GetFiltered")]
        [InlineData("/api/config/flag/first", "GET", typeof(FlagController), "GetFirst")]
        [InlineData("/api/config/flag/previous/1", "GET", typeof(FlagController), "GetPrevious")]
        [InlineData("/api/config/flag/next/1", "GET", typeof(FlagController), "GetNext")]
        [InlineData("/api/config/flag/last", "GET", typeof(FlagController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/flag/custom-fields", "GET", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/config/flag/custom-fields", "GET", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag/custom-fields/{resourceId}", "GET", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/config/flag/custom-fields/{resourceId}", "GET", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag/meta", "HEAD", typeof(FlagController), "GetEntityView")]
        [InlineData("/api/config/flag/meta", "HEAD", typeof(FlagController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/flag/count", "HEAD", typeof(FlagController), "Count")]
        [InlineData("/api/config/flag/count", "HEAD", typeof(FlagController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag/all", "HEAD", typeof(FlagController), "GetAll")]
        [InlineData("/api/config/flag/all", "HEAD", typeof(FlagController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/flag/export", "HEAD", typeof(FlagController), "Export")]
        [InlineData("/api/config/flag/export", "HEAD", typeof(FlagController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/flag/1", "HEAD", typeof(FlagController), "Get")]
        [InlineData("/api/config/flag/1", "HEAD", typeof(FlagController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag/get?flagIds=1", "HEAD", typeof(FlagController), "Get")]
        [InlineData("/api/config/flag/get?flagIds=1", "HEAD", typeof(FlagController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag", "HEAD", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/config/flag", "HEAD", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag/page/1", "HEAD", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/config/flag/page/1", "HEAD", typeof(FlagController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag/count-filtered/{filterName}", "HEAD", typeof(FlagController), "CountFiltered")]
        [InlineData("/api/config/flag/count-filtered/{filterName}", "HEAD", typeof(FlagController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagController), "GetFiltered")]
        [InlineData("/api/config/flag/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagController), "GetFiltered")]
        [InlineData("/api/config/flag/first", "HEAD", typeof(FlagController), "GetFirst")]
        [InlineData("/api/config/flag/previous/1", "HEAD", typeof(FlagController), "GetPrevious")]
        [InlineData("/api/config/flag/next/1", "HEAD", typeof(FlagController), "GetNext")]
        [InlineData("/api/config/flag/last", "HEAD", typeof(FlagController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/flag/custom-fields", "HEAD", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/config/flag/custom-fields", "HEAD", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag/custom-fields/{resourceId}", "HEAD", typeof(FlagController), "GetCustomFields")]
        [InlineData("/api/config/flag/custom-fields/{resourceId}", "HEAD", typeof(FlagController), "GetCustomFields")]

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

        public FlagRouteTests()
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