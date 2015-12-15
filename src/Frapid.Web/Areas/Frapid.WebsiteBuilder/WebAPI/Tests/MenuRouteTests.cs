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
    public class MenuRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/menu/delete/{menuId}", "DELETE", typeof(WebsiteMenuController), "Delete")]
        [InlineData("/api/website/menu/delete/{menuId}", "DELETE", typeof(WebsiteMenuController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/menu/edit/{menuId}", "PUT", typeof(WebsiteMenuController), "Edit")]
        [InlineData("/api/website/menu/edit/{menuId}", "PUT", typeof(WebsiteMenuController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/menu/count-where", "POST", typeof(WebsiteMenuController), "CountWhere")]
        [InlineData("/api/website/menu/count-where", "POST", typeof(WebsiteMenuController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/menu/get-where/{pageNumber}", "POST", typeof(WebsiteMenuController), "GetWhere")]
        [InlineData("/api/website/menu/get-where/{pageNumber}", "POST", typeof(WebsiteMenuController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/menu/add-or-edit", "POST", typeof(WebsiteMenuController), "AddOrEdit")]
        [InlineData("/api/website/menu/add-or-edit", "POST", typeof(WebsiteMenuController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/menu/add/{menu}", "POST", typeof(WebsiteMenuController), "Add")]
        [InlineData("/api/website/menu/add/{menu}", "POST", typeof(WebsiteMenuController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/menu/bulk-import", "POST", typeof(WebsiteMenuController), "BulkImport")]
        [InlineData("/api/website/menu/bulk-import", "POST", typeof(WebsiteMenuController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/menu/meta", "GET", typeof(WebsiteMenuController), "GetEntityView")]
        [InlineData("/api/website/menu/meta", "GET", typeof(WebsiteMenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/menu/count", "GET", typeof(WebsiteMenuController), "Count")]
        [InlineData("/api/website/menu/count", "GET", typeof(WebsiteMenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu/all", "GET", typeof(WebsiteMenuController), "GetAll")]
        [InlineData("/api/website/menu/all", "GET", typeof(WebsiteMenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/menu/export", "GET", typeof(WebsiteMenuController), "Export")]
        [InlineData("/api/website/menu/export", "GET", typeof(WebsiteMenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/menu/1", "GET", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/website/menu/1", "GET", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu/get?menuIds=1", "GET", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/website/menu/get?menuIds=1", "GET", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu", "GET", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/website/menu", "GET", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu/page/1", "GET", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/website/menu/page/1", "GET", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu/count-filtered/{filterName}", "GET", typeof(WebsiteMenuController), "CountFiltered")]
        [InlineData("/api/website/menu/count-filtered/{filterName}", "GET", typeof(WebsiteMenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(WebsiteMenuController), "GetFiltered")]
        [InlineData("/api/website/menu/get-filtered/{pageNumber}/{filterName}", "GET", typeof(WebsiteMenuController), "GetFiltered")]
        [InlineData("/api/website/menu/first", "GET", typeof(WebsiteMenuController), "GetFirst")]
        [InlineData("/api/website/menu/previous/1", "GET", typeof(WebsiteMenuController), "GetPrevious")]
        [InlineData("/api/website/menu/next/1", "GET", typeof(WebsiteMenuController), "GetNext")]
        [InlineData("/api/website/menu/last", "GET", typeof(WebsiteMenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/menu/custom-fields", "GET", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/website/menu/custom-fields", "GET", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu/custom-fields/{resourceId}", "GET", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/website/menu/custom-fields/{resourceId}", "GET", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu/meta", "HEAD", typeof(WebsiteMenuController), "GetEntityView")]
        [InlineData("/api/website/menu/meta", "HEAD", typeof(WebsiteMenuController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/menu/count", "HEAD", typeof(WebsiteMenuController), "Count")]
        [InlineData("/api/website/menu/count", "HEAD", typeof(WebsiteMenuController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/menu/all", "HEAD", typeof(WebsiteMenuController), "GetAll")]
        [InlineData("/api/website/menu/all", "HEAD", typeof(WebsiteMenuController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/menu/export", "HEAD", typeof(WebsiteMenuController), "Export")]
        [InlineData("/api/website/menu/export", "HEAD", typeof(WebsiteMenuController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/menu/1", "HEAD", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/website/menu/1", "HEAD", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu/get?menuIds=1", "HEAD", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/website/menu/get?menuIds=1", "HEAD", typeof(WebsiteMenuController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/menu", "HEAD", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/website/menu", "HEAD", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu/page/1", "HEAD", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/website/menu/page/1", "HEAD", typeof(WebsiteMenuController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/menu/count-filtered/{filterName}", "HEAD", typeof(WebsiteMenuController), "CountFiltered")]
        [InlineData("/api/website/menu/count-filtered/{filterName}", "HEAD", typeof(WebsiteMenuController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(WebsiteMenuController), "GetFiltered")]
        [InlineData("/api/website/menu/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(WebsiteMenuController), "GetFiltered")]
        [InlineData("/api/website/menu/first", "HEAD", typeof(WebsiteMenuController), "GetFirst")]
        [InlineData("/api/website/menu/previous/1", "HEAD", typeof(WebsiteMenuController), "GetPrevious")]
        [InlineData("/api/website/menu/next/1", "HEAD", typeof(WebsiteMenuController), "GetNext")]
        [InlineData("/api/website/menu/last", "HEAD", typeof(WebsiteMenuController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/menu/custom-fields", "HEAD", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/website/menu/custom-fields", "HEAD", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/menu/custom-fields/{resourceId}", "HEAD", typeof(WebsiteMenuController), "GetCustomFields")]
        [InlineData("/api/website/menu/custom-fields/{resourceId}", "HEAD", typeof(WebsiteMenuController), "GetCustomFields")]

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