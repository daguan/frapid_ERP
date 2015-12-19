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

namespace Frapid.Account.Api.Tests
{
    public class RoleRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/role/delete/{roleId}", "DELETE", typeof(RoleController), "Delete")]
        [InlineData("/api/account/role/delete/{roleId}", "DELETE", typeof(RoleController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/role/edit/{roleId}", "PUT", typeof(RoleController), "Edit")]
        [InlineData("/api/account/role/edit/{roleId}", "PUT", typeof(RoleController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/role/count-where", "POST", typeof(RoleController), "CountWhere")]
        [InlineData("/api/account/role/count-where", "POST", typeof(RoleController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/role/get-where/{pageNumber}", "POST", typeof(RoleController), "GetWhere")]
        [InlineData("/api/account/role/get-where/{pageNumber}", "POST", typeof(RoleController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/role/add-or-edit", "POST", typeof(RoleController), "AddOrEdit")]
        [InlineData("/api/account/role/add-or-edit", "POST", typeof(RoleController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/role/add/{role}", "POST", typeof(RoleController), "Add")]
        [InlineData("/api/account/role/add/{role}", "POST", typeof(RoleController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/role/bulk-import", "POST", typeof(RoleController), "BulkImport")]
        [InlineData("/api/account/role/bulk-import", "POST", typeof(RoleController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/role/meta", "GET", typeof(RoleController), "GetEntityView")]
        [InlineData("/api/account/role/meta", "GET", typeof(RoleController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/role/count", "GET", typeof(RoleController), "Count")]
        [InlineData("/api/account/role/count", "GET", typeof(RoleController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/role/all", "GET", typeof(RoleController), "GetAll")]
        [InlineData("/api/account/role/all", "GET", typeof(RoleController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/role/export", "GET", typeof(RoleController), "Export")]
        [InlineData("/api/account/role/export", "GET", typeof(RoleController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/role/1", "GET", typeof(RoleController), "Get")]
        [InlineData("/api/account/role/1", "GET", typeof(RoleController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/role/get?roleIds=1", "GET", typeof(RoleController), "Get")]
        [InlineData("/api/account/role/get?roleIds=1", "GET", typeof(RoleController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/role", "GET", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/account/role", "GET", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/role/page/1", "GET", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/account/role/page/1", "GET", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/role/count-filtered/{filterName}", "GET", typeof(RoleController), "CountFiltered")]
        [InlineData("/api/account/role/count-filtered/{filterName}", "GET", typeof(RoleController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/role/get-filtered/{pageNumber}/{filterName}", "GET", typeof(RoleController), "GetFiltered")]
        [InlineData("/api/account/role/get-filtered/{pageNumber}/{filterName}", "GET", typeof(RoleController), "GetFiltered")]
        [InlineData("/api/account/role/first", "GET", typeof(RoleController), "GetFirst")]
        [InlineData("/api/account/role/previous/1", "GET", typeof(RoleController), "GetPrevious")]
        [InlineData("/api/account/role/next/1", "GET", typeof(RoleController), "GetNext")]
        [InlineData("/api/account/role/last", "GET", typeof(RoleController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/role/custom-fields", "GET", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/account/role/custom-fields", "GET", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/role/custom-fields/{resourceId}", "GET", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/account/role/custom-fields/{resourceId}", "GET", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/role/meta", "HEAD", typeof(RoleController), "GetEntityView")]
        [InlineData("/api/account/role/meta", "HEAD", typeof(RoleController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/role/count", "HEAD", typeof(RoleController), "Count")]
        [InlineData("/api/account/role/count", "HEAD", typeof(RoleController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/role/all", "HEAD", typeof(RoleController), "GetAll")]
        [InlineData("/api/account/role/all", "HEAD", typeof(RoleController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/role/export", "HEAD", typeof(RoleController), "Export")]
        [InlineData("/api/account/role/export", "HEAD", typeof(RoleController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/role/1", "HEAD", typeof(RoleController), "Get")]
        [InlineData("/api/account/role/1", "HEAD", typeof(RoleController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/role/get?roleIds=1", "HEAD", typeof(RoleController), "Get")]
        [InlineData("/api/account/role/get?roleIds=1", "HEAD", typeof(RoleController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/role", "HEAD", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/account/role", "HEAD", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/role/page/1", "HEAD", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/account/role/page/1", "HEAD", typeof(RoleController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/role/count-filtered/{filterName}", "HEAD", typeof(RoleController), "CountFiltered")]
        [InlineData("/api/account/role/count-filtered/{filterName}", "HEAD", typeof(RoleController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/role/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(RoleController), "GetFiltered")]
        [InlineData("/api/account/role/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(RoleController), "GetFiltered")]
        [InlineData("/api/account/role/first", "HEAD", typeof(RoleController), "GetFirst")]
        [InlineData("/api/account/role/previous/1", "HEAD", typeof(RoleController), "GetPrevious")]
        [InlineData("/api/account/role/next/1", "HEAD", typeof(RoleController), "GetNext")]
        [InlineData("/api/account/role/last", "HEAD", typeof(RoleController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/role/custom-fields", "HEAD", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/account/role/custom-fields", "HEAD", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/role/custom-fields/{resourceId}", "HEAD", typeof(RoleController), "GetCustomFields")]
        [InlineData("/api/account/role/custom-fields/{resourceId}", "HEAD", typeof(RoleController), "GetCustomFields")]

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

        public RoleRouteTests()
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