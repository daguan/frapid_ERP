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
    public class EmailQueueRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/delete/{queueId}", "DELETE", typeof(EmailQueueController), "Delete")]
        [InlineData("/api/config/email-queue/delete/{queueId}", "DELETE", typeof(EmailQueueController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/edit/{queueId}", "PUT", typeof(EmailQueueController), "Edit")]
        [InlineData("/api/config/email-queue/edit/{queueId}", "PUT", typeof(EmailQueueController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/count-where", "POST", typeof(EmailQueueController), "CountWhere")]
        [InlineData("/api/config/email-queue/count-where", "POST", typeof(EmailQueueController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/get-where/{pageNumber}", "POST", typeof(EmailQueueController), "GetWhere")]
        [InlineData("/api/config/email-queue/get-where/{pageNumber}", "POST", typeof(EmailQueueController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/add-or-edit", "POST", typeof(EmailQueueController), "AddOrEdit")]
        [InlineData("/api/config/email-queue/add-or-edit", "POST", typeof(EmailQueueController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/add/{emailQueue}", "POST", typeof(EmailQueueController), "Add")]
        [InlineData("/api/config/email-queue/add/{emailQueue}", "POST", typeof(EmailQueueController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/bulk-import", "POST", typeof(EmailQueueController), "BulkImport")]
        [InlineData("/api/config/email-queue/bulk-import", "POST", typeof(EmailQueueController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/meta", "GET", typeof(EmailQueueController), "GetEntityView")]
        [InlineData("/api/config/email-queue/meta", "GET", typeof(EmailQueueController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/count", "GET", typeof(EmailQueueController), "Count")]
        [InlineData("/api/config/email-queue/count", "GET", typeof(EmailQueueController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/all", "GET", typeof(EmailQueueController), "GetAll")]
        [InlineData("/api/config/email-queue/all", "GET", typeof(EmailQueueController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/export", "GET", typeof(EmailQueueController), "Export")]
        [InlineData("/api/config/email-queue/export", "GET", typeof(EmailQueueController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/1", "GET", typeof(EmailQueueController), "Get")]
        [InlineData("/api/config/email-queue/1", "GET", typeof(EmailQueueController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/get?queueIds=1", "GET", typeof(EmailQueueController), "Get")]
        [InlineData("/api/config/email-queue/get?queueIds=1", "GET", typeof(EmailQueueController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue", "GET", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/config/email-queue", "GET", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/page/1", "GET", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/config/email-queue/page/1", "GET", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/count-filtered/{filterName}", "GET", typeof(EmailQueueController), "CountFiltered")]
        [InlineData("/api/config/email-queue/count-filtered/{filterName}", "GET", typeof(EmailQueueController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/get-filtered/{pageNumber}/{filterName}", "GET", typeof(EmailQueueController), "GetFiltered")]
        [InlineData("/api/config/email-queue/get-filtered/{pageNumber}/{filterName}", "GET", typeof(EmailQueueController), "GetFiltered")]
        [InlineData("/api/config/email-queue/first", "GET", typeof(EmailQueueController), "GetFirst")]
        [InlineData("/api/config/email-queue/previous/1", "GET", typeof(EmailQueueController), "GetPrevious")]
        [InlineData("/api/config/email-queue/next/1", "GET", typeof(EmailQueueController), "GetNext")]
        [InlineData("/api/config/email-queue/last", "GET", typeof(EmailQueueController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/email-queue/custom-fields", "GET", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/config/email-queue/custom-fields", "GET", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/custom-fields/{resourceId}", "GET", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/config/email-queue/custom-fields/{resourceId}", "GET", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/meta", "HEAD", typeof(EmailQueueController), "GetEntityView")]
        [InlineData("/api/config/email-queue/meta", "HEAD", typeof(EmailQueueController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/count", "HEAD", typeof(EmailQueueController), "Count")]
        [InlineData("/api/config/email-queue/count", "HEAD", typeof(EmailQueueController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/all", "HEAD", typeof(EmailQueueController), "GetAll")]
        [InlineData("/api/config/email-queue/all", "HEAD", typeof(EmailQueueController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/export", "HEAD", typeof(EmailQueueController), "Export")]
        [InlineData("/api/config/email-queue/export", "HEAD", typeof(EmailQueueController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/1", "HEAD", typeof(EmailQueueController), "Get")]
        [InlineData("/api/config/email-queue/1", "HEAD", typeof(EmailQueueController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/get?queueIds=1", "HEAD", typeof(EmailQueueController), "Get")]
        [InlineData("/api/config/email-queue/get?queueIds=1", "HEAD", typeof(EmailQueueController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue", "HEAD", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/config/email-queue", "HEAD", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/page/1", "HEAD", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/config/email-queue/page/1", "HEAD", typeof(EmailQueueController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/count-filtered/{filterName}", "HEAD", typeof(EmailQueueController), "CountFiltered")]
        [InlineData("/api/config/email-queue/count-filtered/{filterName}", "HEAD", typeof(EmailQueueController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(EmailQueueController), "GetFiltered")]
        [InlineData("/api/config/email-queue/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(EmailQueueController), "GetFiltered")]
        [InlineData("/api/config/email-queue/first", "HEAD", typeof(EmailQueueController), "GetFirst")]
        [InlineData("/api/config/email-queue/previous/1", "HEAD", typeof(EmailQueueController), "GetPrevious")]
        [InlineData("/api/config/email-queue/next/1", "HEAD", typeof(EmailQueueController), "GetNext")]
        [InlineData("/api/config/email-queue/last", "HEAD", typeof(EmailQueueController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/email-queue/custom-fields", "HEAD", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/config/email-queue/custom-fields", "HEAD", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/email-queue/custom-fields/{resourceId}", "HEAD", typeof(EmailQueueController), "GetCustomFields")]
        [InlineData("/api/config/email-queue/custom-fields/{resourceId}", "HEAD", typeof(EmailQueueController), "GetCustomFields")]

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

        public EmailQueueRouteTests()
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