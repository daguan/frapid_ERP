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
    public class KanbanRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/kanban/delete/{kanbanId}", "DELETE", typeof(KanbanController), "Delete")]
        [InlineData("/api/config/kanban/delete/{kanbanId}", "DELETE", typeof(KanbanController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/edit/{kanbanId}", "PUT", typeof(KanbanController), "Edit")]
        [InlineData("/api/config/kanban/edit/{kanbanId}", "PUT", typeof(KanbanController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/count-where", "POST", typeof(KanbanController), "CountWhere")]
        [InlineData("/api/config/kanban/count-where", "POST", typeof(KanbanController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/get-where/{pageNumber}", "POST", typeof(KanbanController), "GetWhere")]
        [InlineData("/api/config/kanban/get-where/{pageNumber}", "POST", typeof(KanbanController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/add-or-edit", "POST", typeof(KanbanController), "AddOrEdit")]
        [InlineData("/api/config/kanban/add-or-edit", "POST", typeof(KanbanController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/add/{kanban}", "POST", typeof(KanbanController), "Add")]
        [InlineData("/api/config/kanban/add/{kanban}", "POST", typeof(KanbanController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/bulk-import", "POST", typeof(KanbanController), "BulkImport")]
        [InlineData("/api/config/kanban/bulk-import", "POST", typeof(KanbanController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/meta", "GET", typeof(KanbanController), "GetEntityView")]
        [InlineData("/api/config/kanban/meta", "GET", typeof(KanbanController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/count", "GET", typeof(KanbanController), "Count")]
        [InlineData("/api/config/kanban/count", "GET", typeof(KanbanController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/all", "GET", typeof(KanbanController), "GetAll")]
        [InlineData("/api/config/kanban/all", "GET", typeof(KanbanController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/export", "GET", typeof(KanbanController), "Export")]
        [InlineData("/api/config/kanban/export", "GET", typeof(KanbanController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/1", "GET", typeof(KanbanController), "Get")]
        [InlineData("/api/config/kanban/1", "GET", typeof(KanbanController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/get?kanbanIds=1", "GET", typeof(KanbanController), "Get")]
        [InlineData("/api/config/kanban/get?kanbanIds=1", "GET", typeof(KanbanController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/kanban", "GET", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/config/kanban", "GET", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/page/1", "GET", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/config/kanban/page/1", "GET", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/count-filtered/{filterName}", "GET", typeof(KanbanController), "CountFiltered")]
        [InlineData("/api/config/kanban/count-filtered/{filterName}", "GET", typeof(KanbanController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/get-filtered/{pageNumber}/{filterName}", "GET", typeof(KanbanController), "GetFiltered")]
        [InlineData("/api/config/kanban/get-filtered/{pageNumber}/{filterName}", "GET", typeof(KanbanController), "GetFiltered")]
        [InlineData("/api/config/kanban/first", "GET", typeof(KanbanController), "GetFirst")]
        [InlineData("/api/config/kanban/previous/1", "GET", typeof(KanbanController), "GetPrevious")]
        [InlineData("/api/config/kanban/next/1", "GET", typeof(KanbanController), "GetNext")]
        [InlineData("/api/config/kanban/last", "GET", typeof(KanbanController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/kanban/custom-fields", "GET", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/config/kanban/custom-fields", "GET", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/custom-fields/{resourceId}", "GET", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/config/kanban/custom-fields/{resourceId}", "GET", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/meta", "HEAD", typeof(KanbanController), "GetEntityView")]
        [InlineData("/api/config/kanban/meta", "HEAD", typeof(KanbanController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/count", "HEAD", typeof(KanbanController), "Count")]
        [InlineData("/api/config/kanban/count", "HEAD", typeof(KanbanController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/all", "HEAD", typeof(KanbanController), "GetAll")]
        [InlineData("/api/config/kanban/all", "HEAD", typeof(KanbanController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/export", "HEAD", typeof(KanbanController), "Export")]
        [InlineData("/api/config/kanban/export", "HEAD", typeof(KanbanController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/1", "HEAD", typeof(KanbanController), "Get")]
        [InlineData("/api/config/kanban/1", "HEAD", typeof(KanbanController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/get?kanbanIds=1", "HEAD", typeof(KanbanController), "Get")]
        [InlineData("/api/config/kanban/get?kanbanIds=1", "HEAD", typeof(KanbanController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/kanban", "HEAD", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/config/kanban", "HEAD", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/page/1", "HEAD", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/config/kanban/page/1", "HEAD", typeof(KanbanController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/count-filtered/{filterName}", "HEAD", typeof(KanbanController), "CountFiltered")]
        [InlineData("/api/config/kanban/count-filtered/{filterName}", "HEAD", typeof(KanbanController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(KanbanController), "GetFiltered")]
        [InlineData("/api/config/kanban/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(KanbanController), "GetFiltered")]
        [InlineData("/api/config/kanban/first", "HEAD", typeof(KanbanController), "GetFirst")]
        [InlineData("/api/config/kanban/previous/1", "HEAD", typeof(KanbanController), "GetPrevious")]
        [InlineData("/api/config/kanban/next/1", "HEAD", typeof(KanbanController), "GetNext")]
        [InlineData("/api/config/kanban/last", "HEAD", typeof(KanbanController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/kanban/custom-fields", "HEAD", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/config/kanban/custom-fields", "HEAD", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/kanban/custom-fields/{resourceId}", "HEAD", typeof(KanbanController), "GetCustomFields")]
        [InlineData("/api/config/kanban/custom-fields/{resourceId}", "HEAD", typeof(KanbanController), "GetCustomFields")]

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

        public KanbanRouteTests()
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