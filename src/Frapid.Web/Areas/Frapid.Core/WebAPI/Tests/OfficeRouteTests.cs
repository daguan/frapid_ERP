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

namespace Frapid.Core.Api.Tests
{
    public class OfficeRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/core/office/delete/{officeId}", "DELETE", typeof(OfficeController), "Delete")]
        [InlineData("/api/core/office/delete/{officeId}", "DELETE", typeof(OfficeController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/core/office/edit/{officeId}", "PUT", typeof(OfficeController), "Edit")]
        [InlineData("/api/core/office/edit/{officeId}", "PUT", typeof(OfficeController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/core/office/count-where", "POST", typeof(OfficeController), "CountWhere")]
        [InlineData("/api/core/office/count-where", "POST", typeof(OfficeController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/core/office/get-where/{pageNumber}", "POST", typeof(OfficeController), "GetWhere")]
        [InlineData("/api/core/office/get-where/{pageNumber}", "POST", typeof(OfficeController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/core/office/add-or-edit", "POST", typeof(OfficeController), "AddOrEdit")]
        [InlineData("/api/core/office/add-or-edit", "POST", typeof(OfficeController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/core/office/add/{office}", "POST", typeof(OfficeController), "Add")]
        [InlineData("/api/core/office/add/{office}", "POST", typeof(OfficeController), "Add")]
        [InlineData("/api/{apiVersionNumber}/core/office/bulk-import", "POST", typeof(OfficeController), "BulkImport")]
        [InlineData("/api/core/office/bulk-import", "POST", typeof(OfficeController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/core/office/meta", "GET", typeof(OfficeController), "GetEntityView")]
        [InlineData("/api/core/office/meta", "GET", typeof(OfficeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/core/office/count", "GET", typeof(OfficeController), "Count")]
        [InlineData("/api/core/office/count", "GET", typeof(OfficeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/core/office/all", "GET", typeof(OfficeController), "GetAll")]
        [InlineData("/api/core/office/all", "GET", typeof(OfficeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/core/office/export", "GET", typeof(OfficeController), "Export")]
        [InlineData("/api/core/office/export", "GET", typeof(OfficeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/core/office/1", "GET", typeof(OfficeController), "Get")]
        [InlineData("/api/core/office/1", "GET", typeof(OfficeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/core/office/get?officeIds=1", "GET", typeof(OfficeController), "Get")]
        [InlineData("/api/core/office/get?officeIds=1", "GET", typeof(OfficeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/core/office", "GET", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/core/office", "GET", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/core/office/page/1", "GET", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/core/office/page/1", "GET", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/core/office/count-filtered/{filterName}", "GET", typeof(OfficeController), "CountFiltered")]
        [InlineData("/api/core/office/count-filtered/{filterName}", "GET", typeof(OfficeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/core/office/get-filtered/{pageNumber}/{filterName}", "GET", typeof(OfficeController), "GetFiltered")]
        [InlineData("/api/core/office/get-filtered/{pageNumber}/{filterName}", "GET", typeof(OfficeController), "GetFiltered")]
        [InlineData("/api/core/office/first", "GET", typeof(OfficeController), "GetFirst")]
        [InlineData("/api/core/office/previous/1", "GET", typeof(OfficeController), "GetPrevious")]
        [InlineData("/api/core/office/next/1", "GET", typeof(OfficeController), "GetNext")]
        [InlineData("/api/core/office/last", "GET", typeof(OfficeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/core/office/custom-fields", "GET", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/core/office/custom-fields", "GET", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/core/office/custom-fields/{resourceId}", "GET", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/core/office/custom-fields/{resourceId}", "GET", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/core/office/meta", "HEAD", typeof(OfficeController), "GetEntityView")]
        [InlineData("/api/core/office/meta", "HEAD", typeof(OfficeController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/core/office/count", "HEAD", typeof(OfficeController), "Count")]
        [InlineData("/api/core/office/count", "HEAD", typeof(OfficeController), "Count")]
        [InlineData("/api/{apiVersionNumber}/core/office/all", "HEAD", typeof(OfficeController), "GetAll")]
        [InlineData("/api/core/office/all", "HEAD", typeof(OfficeController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/core/office/export", "HEAD", typeof(OfficeController), "Export")]
        [InlineData("/api/core/office/export", "HEAD", typeof(OfficeController), "Export")]
        [InlineData("/api/{apiVersionNumber}/core/office/1", "HEAD", typeof(OfficeController), "Get")]
        [InlineData("/api/core/office/1", "HEAD", typeof(OfficeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/core/office/get?officeIds=1", "HEAD", typeof(OfficeController), "Get")]
        [InlineData("/api/core/office/get?officeIds=1", "HEAD", typeof(OfficeController), "Get")]
        [InlineData("/api/{apiVersionNumber}/core/office", "HEAD", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/core/office", "HEAD", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/core/office/page/1", "HEAD", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/core/office/page/1", "HEAD", typeof(OfficeController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/core/office/count-filtered/{filterName}", "HEAD", typeof(OfficeController), "CountFiltered")]
        [InlineData("/api/core/office/count-filtered/{filterName}", "HEAD", typeof(OfficeController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/core/office/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(OfficeController), "GetFiltered")]
        [InlineData("/api/core/office/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(OfficeController), "GetFiltered")]
        [InlineData("/api/core/office/first", "HEAD", typeof(OfficeController), "GetFirst")]
        [InlineData("/api/core/office/previous/1", "HEAD", typeof(OfficeController), "GetPrevious")]
        [InlineData("/api/core/office/next/1", "HEAD", typeof(OfficeController), "GetNext")]
        [InlineData("/api/core/office/last", "HEAD", typeof(OfficeController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/core/office/custom-fields", "HEAD", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/core/office/custom-fields", "HEAD", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/core/office/custom-fields/{resourceId}", "HEAD", typeof(OfficeController), "GetCustomFields")]
        [InlineData("/api/core/office/custom-fields/{resourceId}", "HEAD", typeof(OfficeController), "GetCustomFields")]

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

        public OfficeRouteTests()
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