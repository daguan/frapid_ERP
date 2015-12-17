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
    public class CategoryRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/category/delete/{categoryId}", "DELETE", typeof(CategoryController), "Delete")]
        [InlineData("/api/website/category/delete/{categoryId}", "DELETE", typeof(CategoryController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/category/edit/{categoryId}", "PUT", typeof(CategoryController), "Edit")]
        [InlineData("/api/website/category/edit/{categoryId}", "PUT", typeof(CategoryController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-where", "POST", typeof(CategoryController), "CountWhere")]
        [InlineData("/api/website/category/count-where", "POST", typeof(CategoryController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-where/{pageNumber}", "POST", typeof(CategoryController), "GetWhere")]
        [InlineData("/api/website/category/get-where/{pageNumber}", "POST", typeof(CategoryController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/category/add-or-edit", "POST", typeof(CategoryController), "AddOrEdit")]
        [InlineData("/api/website/category/add-or-edit", "POST", typeof(CategoryController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/category/add/{category}", "POST", typeof(CategoryController), "Add")]
        [InlineData("/api/website/category/add/{category}", "POST", typeof(CategoryController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/category/bulk-import", "POST", typeof(CategoryController), "BulkImport")]
        [InlineData("/api/website/category/bulk-import", "POST", typeof(CategoryController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/category/meta", "GET", typeof(CategoryController), "GetEntityView")]
        [InlineData("/api/website/category/meta", "GET", typeof(CategoryController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/category/count", "GET", typeof(CategoryController), "Count")]
        [InlineData("/api/website/category/count", "GET", typeof(CategoryController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/category/all", "GET", typeof(CategoryController), "GetAll")]
        [InlineData("/api/website/category/all", "GET", typeof(CategoryController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/category/export", "GET", typeof(CategoryController), "Export")]
        [InlineData("/api/website/category/export", "GET", typeof(CategoryController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/category/1", "GET", typeof(CategoryController), "Get")]
        [InlineData("/api/website/category/1", "GET", typeof(CategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category/get?categoryIds=1", "GET", typeof(CategoryController), "Get")]
        [InlineData("/api/website/category/get?categoryIds=1", "GET", typeof(CategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category", "GET", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category", "GET", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/page/1", "GET", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category/page/1", "GET", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-filtered/{filterName}", "GET", typeof(CategoryController), "CountFiltered")]
        [InlineData("/api/website/category/count-filtered/{filterName}", "GET", typeof(CategoryController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-filtered/{pageNumber}/{filterName}", "GET", typeof(CategoryController), "GetFiltered")]
        [InlineData("/api/website/category/get-filtered/{pageNumber}/{filterName}", "GET", typeof(CategoryController), "GetFiltered")]
        [InlineData("/api/website/category/first", "GET", typeof(CategoryController), "GetFirst")]
        [InlineData("/api/website/category/previous/1", "GET", typeof(CategoryController), "GetPrevious")]
        [InlineData("/api/website/category/next/1", "GET", typeof(CategoryController), "GetNext")]
        [InlineData("/api/website/category/last", "GET", typeof(CategoryController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields", "GET", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields", "GET", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields/{resourceId}", "GET", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields/{resourceId}", "GET", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/meta", "HEAD", typeof(CategoryController), "GetEntityView")]
        [InlineData("/api/website/category/meta", "HEAD", typeof(CategoryController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/category/count", "HEAD", typeof(CategoryController), "Count")]
        [InlineData("/api/website/category/count", "HEAD", typeof(CategoryController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/category/all", "HEAD", typeof(CategoryController), "GetAll")]
        [InlineData("/api/website/category/all", "HEAD", typeof(CategoryController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/category/export", "HEAD", typeof(CategoryController), "Export")]
        [InlineData("/api/website/category/export", "HEAD", typeof(CategoryController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/category/1", "HEAD", typeof(CategoryController), "Get")]
        [InlineData("/api/website/category/1", "HEAD", typeof(CategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category/get?categoryIds=1", "HEAD", typeof(CategoryController), "Get")]
        [InlineData("/api/website/category/get?categoryIds=1", "HEAD", typeof(CategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category", "HEAD", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category", "HEAD", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/page/1", "HEAD", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category/page/1", "HEAD", typeof(CategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-filtered/{filterName}", "HEAD", typeof(CategoryController), "CountFiltered")]
        [InlineData("/api/website/category/count-filtered/{filterName}", "HEAD", typeof(CategoryController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(CategoryController), "GetFiltered")]
        [InlineData("/api/website/category/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(CategoryController), "GetFiltered")]
        [InlineData("/api/website/category/first", "HEAD", typeof(CategoryController), "GetFirst")]
        [InlineData("/api/website/category/previous/1", "HEAD", typeof(CategoryController), "GetPrevious")]
        [InlineData("/api/website/category/next/1", "HEAD", typeof(CategoryController), "GetNext")]
        [InlineData("/api/website/category/last", "HEAD", typeof(CategoryController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields", "HEAD", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields", "HEAD", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields/{resourceId}", "HEAD", typeof(CategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields/{resourceId}", "HEAD", typeof(CategoryController), "GetCustomFields")]

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

        public CategoryRouteTests()
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