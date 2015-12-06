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
    public class FlagTypeRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/delete/{flagTypeId}", "DELETE", typeof(FlagTypeController), "Delete")]
        [InlineData("/api/config/flag-type/delete/{flagTypeId}", "DELETE", typeof(FlagTypeController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/edit/{flagTypeId}", "PUT", typeof(FlagTypeController), "Edit")]
        [InlineData("/api/config/flag-type/edit/{flagTypeId}", "PUT", typeof(FlagTypeController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/count-where", "POST", typeof(FlagTypeController), "CountWhere")]
        [InlineData("/api/config/flag-type/count-where", "POST", typeof(FlagTypeController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/get-where/{pageNumber}", "POST", typeof(FlagTypeController), "GetWhere")]
        [InlineData("/api/config/flag-type/get-where/{pageNumber}", "POST", typeof(FlagTypeController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/add-or-edit", "POST", typeof(FlagTypeController), "AddOrEdit")]
        [InlineData("/api/config/flag-type/add-or-edit", "POST", typeof(FlagTypeController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/add/{flagType}", "POST", typeof(FlagTypeController), "Add")]
        [InlineData("/api/config/flag-type/add/{flagType}", "POST", typeof(FlagTypeController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/bulk-import", "POST", typeof(FlagTypeController), "BulkImport")]
        [InlineData("/api/config/flag-type/bulk-import", "POST", typeof(FlagTypeController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/meta", "GET", typeof(FlagTypeController), "GetEntityView")]
        [InlineData("/api/config/flag-type/meta", "GET", typeof(FlagTypeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/count", "GET", typeof(FlagTypeController), "Count")]
        [InlineData("/api/config/flag-type/count", "GET", typeof(FlagTypeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/all", "GET", typeof(FlagTypeController), "GetAll")]
        [InlineData("/api/config/flag-type/all", "GET", typeof(FlagTypeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/export", "GET", typeof(FlagTypeController), "Export")]
        [InlineData("/api/config/flag-type/export", "GET", typeof(FlagTypeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/1", "GET", typeof(FlagTypeController), "Get")]
        [InlineData("/api/config/flag-type/1", "GET", typeof(FlagTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/get?flagTypeIds=1", "GET", typeof(FlagTypeController), "Get")]
        [InlineData("/api/config/flag-type/get?flagTypeIds=1", "GET", typeof(FlagTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type", "GET", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-type", "GET", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/page/1", "GET", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-type/page/1", "GET", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/count-filtered/{filterName}", "GET", typeof(FlagTypeController), "CountFiltered")]
        [InlineData("/api/config/flag-type/count-filtered/{filterName}", "GET", typeof(FlagTypeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagTypeController), "GetFiltered")]
        [InlineData("/api/config/flag-type/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FlagTypeController), "GetFiltered")]
        [InlineData("/api/config/flag-type/first", "GET", typeof(FlagTypeController), "GetFirst")]
        [InlineData("/api/config/flag-type/previous/1", "GET", typeof(FlagTypeController), "GetPrevious")]
        [InlineData("/api/config/flag-type/next/1", "GET", typeof(FlagTypeController), "GetNext")]
        [InlineData("/api/config/flag-type/last", "GET", typeof(FlagTypeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/flag-type/custom-fields", "GET", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/config/flag-type/custom-fields", "GET", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/custom-fields/{resourceId}", "GET", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/config/flag-type/custom-fields/{resourceId}", "GET", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/meta", "HEAD", typeof(FlagTypeController), "GetEntityView")]
        [InlineData("/api/config/flag-type/meta", "HEAD", typeof(FlagTypeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/count", "HEAD", typeof(FlagTypeController), "Count")]
        [InlineData("/api/config/flag-type/count", "HEAD", typeof(FlagTypeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/all", "HEAD", typeof(FlagTypeController), "GetAll")]
        [InlineData("/api/config/flag-type/all", "HEAD", typeof(FlagTypeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/export", "HEAD", typeof(FlagTypeController), "Export")]
        [InlineData("/api/config/flag-type/export", "HEAD", typeof(FlagTypeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/1", "HEAD", typeof(FlagTypeController), "Get")]
        [InlineData("/api/config/flag-type/1", "HEAD", typeof(FlagTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/get?flagTypeIds=1", "HEAD", typeof(FlagTypeController), "Get")]
        [InlineData("/api/config/flag-type/get?flagTypeIds=1", "HEAD", typeof(FlagTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type", "HEAD", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-type", "HEAD", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/page/1", "HEAD", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/flag-type/page/1", "HEAD", typeof(FlagTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/count-filtered/{filterName}", "HEAD", typeof(FlagTypeController), "CountFiltered")]
        [InlineData("/api/config/flag-type/count-filtered/{filterName}", "HEAD", typeof(FlagTypeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagTypeController), "GetFiltered")]
        [InlineData("/api/config/flag-type/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FlagTypeController), "GetFiltered")]
        [InlineData("/api/config/flag-type/first", "HEAD", typeof(FlagTypeController), "GetFirst")]
        [InlineData("/api/config/flag-type/previous/1", "HEAD", typeof(FlagTypeController), "GetPrevious")]
        [InlineData("/api/config/flag-type/next/1", "HEAD", typeof(FlagTypeController), "GetNext")]
        [InlineData("/api/config/flag-type/last", "HEAD", typeof(FlagTypeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/flag-type/custom-fields", "HEAD", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/config/flag-type/custom-fields", "HEAD", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/flag-type/custom-fields/{resourceId}", "HEAD", typeof(FlagTypeController), "GetCustomFields")]
        [InlineData("/api/config/flag-type/custom-fields/{resourceId}", "HEAD", typeof(FlagTypeController), "GetCustomFields")]

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

        public FlagTypeRouteTests()
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