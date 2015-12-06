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
    public class FilterRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/filter/delete/{filterId}", "DELETE", typeof(FilterController), "Delete")]
        [InlineData("/api/config/filter/delete/{filterId}", "DELETE", typeof(FilterController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/filter/edit/{filterId}", "PUT", typeof(FilterController), "Edit")]
        [InlineData("/api/config/filter/edit/{filterId}", "PUT", typeof(FilterController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/filter/count-where", "POST", typeof(FilterController), "CountWhere")]
        [InlineData("/api/config/filter/count-where", "POST", typeof(FilterController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/filter/get-where/{pageNumber}", "POST", typeof(FilterController), "GetWhere")]
        [InlineData("/api/config/filter/get-where/{pageNumber}", "POST", typeof(FilterController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/filter/add-or-edit", "POST", typeof(FilterController), "AddOrEdit")]
        [InlineData("/api/config/filter/add-or-edit", "POST", typeof(FilterController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/filter/add/{filter}", "POST", typeof(FilterController), "Add")]
        [InlineData("/api/config/filter/add/{filter}", "POST", typeof(FilterController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/filter/bulk-import", "POST", typeof(FilterController), "BulkImport")]
        [InlineData("/api/config/filter/bulk-import", "POST", typeof(FilterController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/filter/meta", "GET", typeof(FilterController), "GetEntityView")]
        [InlineData("/api/config/filter/meta", "GET", typeof(FilterController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/filter/count", "GET", typeof(FilterController), "Count")]
        [InlineData("/api/config/filter/count", "GET", typeof(FilterController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/filter/all", "GET", typeof(FilterController), "GetAll")]
        [InlineData("/api/config/filter/all", "GET", typeof(FilterController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/filter/export", "GET", typeof(FilterController), "Export")]
        [InlineData("/api/config/filter/export", "GET", typeof(FilterController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/filter/1", "GET", typeof(FilterController), "Get")]
        [InlineData("/api/config/filter/1", "GET", typeof(FilterController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/filter/get?filterIds=1", "GET", typeof(FilterController), "Get")]
        [InlineData("/api/config/filter/get?filterIds=1", "GET", typeof(FilterController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/filter", "GET", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/config/filter", "GET", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/filter/page/1", "GET", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/config/filter/page/1", "GET", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/filter/count-filtered/{filterName}", "GET", typeof(FilterController), "CountFiltered")]
        [InlineData("/api/config/filter/count-filtered/{filterName}", "GET", typeof(FilterController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/filter/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FilterController), "GetFiltered")]
        [InlineData("/api/config/filter/get-filtered/{pageNumber}/{filterName}", "GET", typeof(FilterController), "GetFiltered")]
        [InlineData("/api/config/filter/first", "GET", typeof(FilterController), "GetFirst")]
        [InlineData("/api/config/filter/previous/1", "GET", typeof(FilterController), "GetPrevious")]
        [InlineData("/api/config/filter/next/1", "GET", typeof(FilterController), "GetNext")]
        [InlineData("/api/config/filter/last", "GET", typeof(FilterController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/filter/custom-fields", "GET", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/config/filter/custom-fields", "GET", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/filter/custom-fields/{resourceId}", "GET", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/config/filter/custom-fields/{resourceId}", "GET", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/filter/meta", "HEAD", typeof(FilterController), "GetEntityView")]
        [InlineData("/api/config/filter/meta", "HEAD", typeof(FilterController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/filter/count", "HEAD", typeof(FilterController), "Count")]
        [InlineData("/api/config/filter/count", "HEAD", typeof(FilterController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/filter/all", "HEAD", typeof(FilterController), "GetAll")]
        [InlineData("/api/config/filter/all", "HEAD", typeof(FilterController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/filter/export", "HEAD", typeof(FilterController), "Export")]
        [InlineData("/api/config/filter/export", "HEAD", typeof(FilterController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/filter/1", "HEAD", typeof(FilterController), "Get")]
        [InlineData("/api/config/filter/1", "HEAD", typeof(FilterController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/filter/get?filterIds=1", "HEAD", typeof(FilterController), "Get")]
        [InlineData("/api/config/filter/get?filterIds=1", "HEAD", typeof(FilterController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/filter", "HEAD", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/config/filter", "HEAD", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/filter/page/1", "HEAD", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/config/filter/page/1", "HEAD", typeof(FilterController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/filter/count-filtered/{filterName}", "HEAD", typeof(FilterController), "CountFiltered")]
        [InlineData("/api/config/filter/count-filtered/{filterName}", "HEAD", typeof(FilterController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/filter/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FilterController), "GetFiltered")]
        [InlineData("/api/config/filter/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(FilterController), "GetFiltered")]
        [InlineData("/api/config/filter/first", "HEAD", typeof(FilterController), "GetFirst")]
        [InlineData("/api/config/filter/previous/1", "HEAD", typeof(FilterController), "GetPrevious")]
        [InlineData("/api/config/filter/next/1", "HEAD", typeof(FilterController), "GetNext")]
        [InlineData("/api/config/filter/last", "HEAD", typeof(FilterController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/filter/custom-fields", "HEAD", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/config/filter/custom-fields", "HEAD", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/filter/custom-fields/{resourceId}", "HEAD", typeof(FilterController), "GetCustomFields")]
        [InlineData("/api/config/filter/custom-fields/{resourceId}", "HEAD", typeof(FilterController), "GetCustomFields")]

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

        public FilterRouteTests()
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