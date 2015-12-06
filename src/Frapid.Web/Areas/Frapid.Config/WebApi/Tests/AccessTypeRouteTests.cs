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
    public class AccessTypeRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/access-type/delete/{accessTypeId}", "DELETE", typeof(AccessTypeController), "Delete")]
        [InlineData("/api/config/access-type/delete/{accessTypeId}", "DELETE", typeof(AccessTypeController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/edit/{accessTypeId}", "PUT", typeof(AccessTypeController), "Edit")]
        [InlineData("/api/config/access-type/edit/{accessTypeId}", "PUT", typeof(AccessTypeController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/count-where", "POST", typeof(AccessTypeController), "CountWhere")]
        [InlineData("/api/config/access-type/count-where", "POST", typeof(AccessTypeController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/get-where/{pageNumber}", "POST", typeof(AccessTypeController), "GetWhere")]
        [InlineData("/api/config/access-type/get-where/{pageNumber}", "POST", typeof(AccessTypeController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/add-or-edit", "POST", typeof(AccessTypeController), "AddOrEdit")]
        [InlineData("/api/config/access-type/add-or-edit", "POST", typeof(AccessTypeController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/add/{accessType}", "POST", typeof(AccessTypeController), "Add")]
        [InlineData("/api/config/access-type/add/{accessType}", "POST", typeof(AccessTypeController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/bulk-import", "POST", typeof(AccessTypeController), "BulkImport")]
        [InlineData("/api/config/access-type/bulk-import", "POST", typeof(AccessTypeController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/meta", "GET", typeof(AccessTypeController), "GetEntityView")]
        [InlineData("/api/config/access-type/meta", "GET", typeof(AccessTypeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/count", "GET", typeof(AccessTypeController), "Count")]
        [InlineData("/api/config/access-type/count", "GET", typeof(AccessTypeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/all", "GET", typeof(AccessTypeController), "GetAll")]
        [InlineData("/api/config/access-type/all", "GET", typeof(AccessTypeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/export", "GET", typeof(AccessTypeController), "Export")]
        [InlineData("/api/config/access-type/export", "GET", typeof(AccessTypeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/1", "GET", typeof(AccessTypeController), "Get")]
        [InlineData("/api/config/access-type/1", "GET", typeof(AccessTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/get?accessTypeIds=1", "GET", typeof(AccessTypeController), "Get")]
        [InlineData("/api/config/access-type/get?accessTypeIds=1", "GET", typeof(AccessTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/access-type", "GET", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/access-type", "GET", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/page/1", "GET", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/access-type/page/1", "GET", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/count-filtered/{filterName}", "GET", typeof(AccessTypeController), "CountFiltered")]
        [InlineData("/api/config/access-type/count-filtered/{filterName}", "GET", typeof(AccessTypeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AccessTypeController), "GetFiltered")]
        [InlineData("/api/config/access-type/get-filtered/{pageNumber}/{filterName}", "GET", typeof(AccessTypeController), "GetFiltered")]
        [InlineData("/api/config/access-type/first", "GET", typeof(AccessTypeController), "GetFirst")]
        [InlineData("/api/config/access-type/previous/1", "GET", typeof(AccessTypeController), "GetPrevious")]
        [InlineData("/api/config/access-type/next/1", "GET", typeof(AccessTypeController), "GetNext")]
        [InlineData("/api/config/access-type/last", "GET", typeof(AccessTypeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/access-type/custom-fields", "GET", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/config/access-type/custom-fields", "GET", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/custom-fields/{resourceId}", "GET", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/config/access-type/custom-fields/{resourceId}", "GET", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/meta", "HEAD", typeof(AccessTypeController), "GetEntityView")]
        [InlineData("/api/config/access-type/meta", "HEAD", typeof(AccessTypeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/count", "HEAD", typeof(AccessTypeController), "Count")]
        [InlineData("/api/config/access-type/count", "HEAD", typeof(AccessTypeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/all", "HEAD", typeof(AccessTypeController), "GetAll")]
        [InlineData("/api/config/access-type/all", "HEAD", typeof(AccessTypeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/export", "HEAD", typeof(AccessTypeController), "Export")]
        [InlineData("/api/config/access-type/export", "HEAD", typeof(AccessTypeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/1", "HEAD", typeof(AccessTypeController), "Get")]
        [InlineData("/api/config/access-type/1", "HEAD", typeof(AccessTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/get?accessTypeIds=1", "HEAD", typeof(AccessTypeController), "Get")]
        [InlineData("/api/config/access-type/get?accessTypeIds=1", "HEAD", typeof(AccessTypeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/access-type", "HEAD", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/access-type", "HEAD", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/page/1", "HEAD", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/config/access-type/page/1", "HEAD", typeof(AccessTypeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/count-filtered/{filterName}", "HEAD", typeof(AccessTypeController), "CountFiltered")]
        [InlineData("/api/config/access-type/count-filtered/{filterName}", "HEAD", typeof(AccessTypeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AccessTypeController), "GetFiltered")]
        [InlineData("/api/config/access-type/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(AccessTypeController), "GetFiltered")]
        [InlineData("/api/config/access-type/first", "HEAD", typeof(AccessTypeController), "GetFirst")]
        [InlineData("/api/config/access-type/previous/1", "HEAD", typeof(AccessTypeController), "GetPrevious")]
        [InlineData("/api/config/access-type/next/1", "HEAD", typeof(AccessTypeController), "GetNext")]
        [InlineData("/api/config/access-type/last", "HEAD", typeof(AccessTypeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/access-type/custom-fields", "HEAD", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/config/access-type/custom-fields", "HEAD", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/access-type/custom-fields/{resourceId}", "HEAD", typeof(AccessTypeController), "GetCustomFields")]
        [InlineData("/api/config/access-type/custom-fields/{resourceId}", "HEAD", typeof(AccessTypeController), "GetCustomFields")]

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

        public AccessTypeRouteTests()
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