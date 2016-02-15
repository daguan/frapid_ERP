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
        [InlineData("/api/{apiVersionNumber}/website/category/delete/{categoryId}", "DELETE", typeof(WebsiteCategoryController), "Delete")]
        [InlineData("/api/website/category/delete/{categoryId}", "DELETE", typeof(WebsiteCategoryController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/category/edit/{categoryId}", "PUT", typeof(WebsiteCategoryController), "Edit")]
        [InlineData("/api/website/category/edit/{categoryId}", "PUT", typeof(WebsiteCategoryController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-where", "POST", typeof(WebsiteCategoryController), "CountWhere")]
        [InlineData("/api/website/category/count-where", "POST", typeof(WebsiteCategoryController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-where/{pageNumber}", "POST", typeof(WebsiteCategoryController), "GetWhere")]
        [InlineData("/api/website/category/get-where/{pageNumber}", "POST", typeof(WebsiteCategoryController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/category/add-or-edit", "POST", typeof(WebsiteCategoryController), "AddOrEdit")]
        [InlineData("/api/website/category/add-or-edit", "POST", typeof(WebsiteCategoryController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/category/add/{category}", "POST", typeof(WebsiteCategoryController), "Add")]
        [InlineData("/api/website/category/add/{category}", "POST", typeof(WebsiteCategoryController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/category/bulk-import", "POST", typeof(WebsiteCategoryController), "BulkImport")]
        [InlineData("/api/website/category/bulk-import", "POST", typeof(WebsiteCategoryController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/category/meta", "GET", typeof(WebsiteCategoryController), "GetEntityView")]
        [InlineData("/api/website/category/meta", "GET", typeof(WebsiteCategoryController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/category/count", "GET", typeof(WebsiteCategoryController), "Count")]
        [InlineData("/api/website/category/count", "GET", typeof(WebsiteCategoryController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/category/all", "GET", typeof(WebsiteCategoryController), "GetAll")]
        [InlineData("/api/website/category/all", "GET", typeof(WebsiteCategoryController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/category/export", "GET", typeof(WebsiteCategoryController), "Export")]
        [InlineData("/api/website/category/export", "GET", typeof(WebsiteCategoryController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/category/1", "GET", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/website/category/1", "GET", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category/get?categoryIds=1", "GET", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/website/category/get?categoryIds=1", "GET", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category", "GET", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category", "GET", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/page/1", "GET", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category/page/1", "GET", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-filtered/{filterName}", "GET", typeof(WebsiteCategoryController), "CountFiltered")]
        [InlineData("/api/website/category/count-filtered/{filterName}", "GET", typeof(WebsiteCategoryController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-filtered/{pageNumber}/{filterName}", "GET", typeof(WebsiteCategoryController), "GetFiltered")]
        [InlineData("/api/website/category/get-filtered/{pageNumber}/{filterName}", "GET", typeof(WebsiteCategoryController), "GetFiltered")]
        [InlineData("/api/website/category/first", "GET", typeof(WebsiteCategoryController), "GetFirst")]
        [InlineData("/api/website/category/previous/1", "GET", typeof(WebsiteCategoryController), "GetPrevious")]
        [InlineData("/api/website/category/next/1", "GET", typeof(WebsiteCategoryController), "GetNext")]
        [InlineData("/api/website/category/last", "GET", typeof(WebsiteCategoryController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields", "GET", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields", "GET", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields/{resourceId}", "GET", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields/{resourceId}", "GET", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/meta", "HEAD", typeof(WebsiteCategoryController), "GetEntityView")]
        [InlineData("/api/website/category/meta", "HEAD", typeof(WebsiteCategoryController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/category/count", "HEAD", typeof(WebsiteCategoryController), "Count")]
        [InlineData("/api/website/category/count", "HEAD", typeof(WebsiteCategoryController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/category/all", "HEAD", typeof(WebsiteCategoryController), "GetAll")]
        [InlineData("/api/website/category/all", "HEAD", typeof(WebsiteCategoryController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/category/export", "HEAD", typeof(WebsiteCategoryController), "Export")]
        [InlineData("/api/website/category/export", "HEAD", typeof(WebsiteCategoryController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/category/1", "HEAD", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/website/category/1", "HEAD", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category/get?categoryIds=1", "HEAD", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/website/category/get?categoryIds=1", "HEAD", typeof(WebsiteCategoryController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/category", "HEAD", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category", "HEAD", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/page/1", "HEAD", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/website/category/page/1", "HEAD", typeof(WebsiteCategoryController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/category/count-filtered/{filterName}", "HEAD", typeof(WebsiteCategoryController), "CountFiltered")]
        [InlineData("/api/website/category/count-filtered/{filterName}", "HEAD", typeof(WebsiteCategoryController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/category/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(WebsiteCategoryController), "GetFiltered")]
        [InlineData("/api/website/category/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(WebsiteCategoryController), "GetFiltered")]
        [InlineData("/api/website/category/first", "HEAD", typeof(WebsiteCategoryController), "GetFirst")]
        [InlineData("/api/website/category/previous/1", "HEAD", typeof(WebsiteCategoryController), "GetPrevious")]
        [InlineData("/api/website/category/next/1", "HEAD", typeof(WebsiteCategoryController), "GetNext")]
        [InlineData("/api/website/category/last", "HEAD", typeof(WebsiteCategoryController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields", "HEAD", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields", "HEAD", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/category/custom-fields/{resourceId}", "HEAD", typeof(WebsiteCategoryController), "GetCustomFields")]
        [InlineData("/api/website/category/custom-fields/{resourceId}", "HEAD", typeof(WebsiteCategoryController), "GetCustomFields")]

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