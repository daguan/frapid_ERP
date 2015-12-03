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
    public class ContentRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/wb/content/delete/{contentId}", "DELETE", typeof(ContentController), "Delete")]
        [InlineData("/api/wb/content/delete/{contentId}", "DELETE", typeof(ContentController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/wb/content/edit/{contentId}", "PUT", typeof(ContentController), "Edit")]
        [InlineData("/api/wb/content/edit/{contentId}", "PUT", typeof(ContentController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/wb/content/count-where", "POST", typeof(ContentController), "CountWhere")]
        [InlineData("/api/wb/content/count-where", "POST", typeof(ContentController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/wb/content/get-where/{pageNumber}", "POST", typeof(ContentController), "GetWhere")]
        [InlineData("/api/wb/content/get-where/{pageNumber}", "POST", typeof(ContentController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/wb/content/add-or-edit", "POST", typeof(ContentController), "AddOrEdit")]
        [InlineData("/api/wb/content/add-or-edit", "POST", typeof(ContentController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/wb/content/add/{content}", "POST", typeof(ContentController), "Add")]
        [InlineData("/api/wb/content/add/{content}", "POST", typeof(ContentController), "Add")]
        [InlineData("/api/{apiVersionNumber}/wb/content/bulk-import", "POST", typeof(ContentController), "BulkImport")]
        [InlineData("/api/wb/content/bulk-import", "POST", typeof(ContentController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/wb/content/meta", "GET", typeof(ContentController), "GetEntityView")]
        [InlineData("/api/wb/content/meta", "GET", typeof(ContentController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/wb/content/count", "GET", typeof(ContentController), "Count")]
        [InlineData("/api/wb/content/count", "GET", typeof(ContentController), "Count")]
        [InlineData("/api/{apiVersionNumber}/wb/content/all", "GET", typeof(ContentController), "GetAll")]
        [InlineData("/api/wb/content/all", "GET", typeof(ContentController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/wb/content/export", "GET", typeof(ContentController), "Export")]
        [InlineData("/api/wb/content/export", "GET", typeof(ContentController), "Export")]
        [InlineData("/api/{apiVersionNumber}/wb/content/1", "GET", typeof(ContentController), "Get")]
        [InlineData("/api/wb/content/1", "GET", typeof(ContentController), "Get")]
        [InlineData("/api/{apiVersionNumber}/wb/content/get?contentIds=1", "GET", typeof(ContentController), "Get")]
        [InlineData("/api/wb/content/get?contentIds=1", "GET", typeof(ContentController), "Get")]
        [InlineData("/api/{apiVersionNumber}/wb/content", "GET", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/wb/content", "GET", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/wb/content/page/1", "GET", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/wb/content/page/1", "GET", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/wb/content/count-filtered/{filterName}", "GET", typeof(ContentController), "CountFiltered")]
        [InlineData("/api/wb/content/count-filtered/{filterName}", "GET", typeof(ContentController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/wb/content/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ContentController), "GetFiltered")]
        [InlineData("/api/wb/content/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ContentController), "GetFiltered")]
        [InlineData("/api/wb/content/first", "GET", typeof(ContentController), "GetFirst")]
        [InlineData("/api/wb/content/previous/1", "GET", typeof(ContentController), "GetPrevious")]
        [InlineData("/api/wb/content/next/1", "GET", typeof(ContentController), "GetNext")]
        [InlineData("/api/wb/content/last", "GET", typeof(ContentController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/wb/content/custom-fields", "GET", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/wb/content/custom-fields", "GET", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/wb/content/custom-fields/{resourceId}", "GET", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/wb/content/custom-fields/{resourceId}", "GET", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/wb/content/meta", "HEAD", typeof(ContentController), "GetEntityView")]
        [InlineData("/api/wb/content/meta", "HEAD", typeof(ContentController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/wb/content/count", "HEAD", typeof(ContentController), "Count")]
        [InlineData("/api/wb/content/count", "HEAD", typeof(ContentController), "Count")]
        [InlineData("/api/{apiVersionNumber}/wb/content/all", "HEAD", typeof(ContentController), "GetAll")]
        [InlineData("/api/wb/content/all", "HEAD", typeof(ContentController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/wb/content/export", "HEAD", typeof(ContentController), "Export")]
        [InlineData("/api/wb/content/export", "HEAD", typeof(ContentController), "Export")]
        [InlineData("/api/{apiVersionNumber}/wb/content/1", "HEAD", typeof(ContentController), "Get")]
        [InlineData("/api/wb/content/1", "HEAD", typeof(ContentController), "Get")]
        [InlineData("/api/{apiVersionNumber}/wb/content/get?contentIds=1", "HEAD", typeof(ContentController), "Get")]
        [InlineData("/api/wb/content/get?contentIds=1", "HEAD", typeof(ContentController), "Get")]
        [InlineData("/api/{apiVersionNumber}/wb/content", "HEAD", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/wb/content", "HEAD", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/wb/content/page/1", "HEAD", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/wb/content/page/1", "HEAD", typeof(ContentController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/wb/content/count-filtered/{filterName}", "HEAD", typeof(ContentController), "CountFiltered")]
        [InlineData("/api/wb/content/count-filtered/{filterName}", "HEAD", typeof(ContentController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/wb/content/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ContentController), "GetFiltered")]
        [InlineData("/api/wb/content/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ContentController), "GetFiltered")]
        [InlineData("/api/wb/content/first", "HEAD", typeof(ContentController), "GetFirst")]
        [InlineData("/api/wb/content/previous/1", "HEAD", typeof(ContentController), "GetPrevious")]
        [InlineData("/api/wb/content/next/1", "HEAD", typeof(ContentController), "GetNext")]
        [InlineData("/api/wb/content/last", "HEAD", typeof(ContentController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/wb/content/custom-fields", "HEAD", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/wb/content/custom-fields", "HEAD", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/wb/content/custom-fields/{resourceId}", "HEAD", typeof(ContentController), "GetCustomFields")]
        [InlineData("/api/wb/content/custom-fields/{resourceId}", "HEAD", typeof(ContentController), "GetCustomFields")]

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

        public ContentRouteTests()
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