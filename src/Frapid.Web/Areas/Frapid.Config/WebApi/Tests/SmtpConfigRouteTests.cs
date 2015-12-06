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
    public class SmtpConfigRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/delete/{smtpId}", "DELETE", typeof(SmtpConfigController), "Delete")]
        [InlineData("/api/config/smtp-config/delete/{smtpId}", "DELETE", typeof(SmtpConfigController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/edit/{smtpId}", "PUT", typeof(SmtpConfigController), "Edit")]
        [InlineData("/api/config/smtp-config/edit/{smtpId}", "PUT", typeof(SmtpConfigController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/count-where", "POST", typeof(SmtpConfigController), "CountWhere")]
        [InlineData("/api/config/smtp-config/count-where", "POST", typeof(SmtpConfigController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/get-where/{pageNumber}", "POST", typeof(SmtpConfigController), "GetWhere")]
        [InlineData("/api/config/smtp-config/get-where/{pageNumber}", "POST", typeof(SmtpConfigController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/add-or-edit", "POST", typeof(SmtpConfigController), "AddOrEdit")]
        [InlineData("/api/config/smtp-config/add-or-edit", "POST", typeof(SmtpConfigController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/add/{smtpConfig}", "POST", typeof(SmtpConfigController), "Add")]
        [InlineData("/api/config/smtp-config/add/{smtpConfig}", "POST", typeof(SmtpConfigController), "Add")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/bulk-import", "POST", typeof(SmtpConfigController), "BulkImport")]
        [InlineData("/api/config/smtp-config/bulk-import", "POST", typeof(SmtpConfigController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/meta", "GET", typeof(SmtpConfigController), "GetEntityView")]
        [InlineData("/api/config/smtp-config/meta", "GET", typeof(SmtpConfigController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/count", "GET", typeof(SmtpConfigController), "Count")]
        [InlineData("/api/config/smtp-config/count", "GET", typeof(SmtpConfigController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/all", "GET", typeof(SmtpConfigController), "GetAll")]
        [InlineData("/api/config/smtp-config/all", "GET", typeof(SmtpConfigController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/export", "GET", typeof(SmtpConfigController), "Export")]
        [InlineData("/api/config/smtp-config/export", "GET", typeof(SmtpConfigController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/1", "GET", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/config/smtp-config/1", "GET", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/get?smtpIds=1", "GET", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/config/smtp-config/get?smtpIds=1", "GET", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config", "GET", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/config/smtp-config", "GET", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/page/1", "GET", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/config/smtp-config/page/1", "GET", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/count-filtered/{filterName}", "GET", typeof(SmtpConfigController), "CountFiltered")]
        [InlineData("/api/config/smtp-config/count-filtered/{filterName}", "GET", typeof(SmtpConfigController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/get-filtered/{pageNumber}/{filterName}", "GET", typeof(SmtpConfigController), "GetFiltered")]
        [InlineData("/api/config/smtp-config/get-filtered/{pageNumber}/{filterName}", "GET", typeof(SmtpConfigController), "GetFiltered")]
        [InlineData("/api/config/smtp-config/first", "GET", typeof(SmtpConfigController), "GetFirst")]
        [InlineData("/api/config/smtp-config/previous/1", "GET", typeof(SmtpConfigController), "GetPrevious")]
        [InlineData("/api/config/smtp-config/next/1", "GET", typeof(SmtpConfigController), "GetNext")]
        [InlineData("/api/config/smtp-config/last", "GET", typeof(SmtpConfigController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/smtp-config/custom-fields", "GET", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/config/smtp-config/custom-fields", "GET", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/custom-fields/{resourceId}", "GET", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/config/smtp-config/custom-fields/{resourceId}", "GET", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/meta", "HEAD", typeof(SmtpConfigController), "GetEntityView")]
        [InlineData("/api/config/smtp-config/meta", "HEAD", typeof(SmtpConfigController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/count", "HEAD", typeof(SmtpConfigController), "Count")]
        [InlineData("/api/config/smtp-config/count", "HEAD", typeof(SmtpConfigController), "Count")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/all", "HEAD", typeof(SmtpConfigController), "GetAll")]
        [InlineData("/api/config/smtp-config/all", "HEAD", typeof(SmtpConfigController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/export", "HEAD", typeof(SmtpConfigController), "Export")]
        [InlineData("/api/config/smtp-config/export", "HEAD", typeof(SmtpConfigController), "Export")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/1", "HEAD", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/config/smtp-config/1", "HEAD", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/get?smtpIds=1", "HEAD", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/config/smtp-config/get?smtpIds=1", "HEAD", typeof(SmtpConfigController), "Get")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config", "HEAD", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/config/smtp-config", "HEAD", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/page/1", "HEAD", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/config/smtp-config/page/1", "HEAD", typeof(SmtpConfigController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/count-filtered/{filterName}", "HEAD", typeof(SmtpConfigController), "CountFiltered")]
        [InlineData("/api/config/smtp-config/count-filtered/{filterName}", "HEAD", typeof(SmtpConfigController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(SmtpConfigController), "GetFiltered")]
        [InlineData("/api/config/smtp-config/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(SmtpConfigController), "GetFiltered")]
        [InlineData("/api/config/smtp-config/first", "HEAD", typeof(SmtpConfigController), "GetFirst")]
        [InlineData("/api/config/smtp-config/previous/1", "HEAD", typeof(SmtpConfigController), "GetPrevious")]
        [InlineData("/api/config/smtp-config/next/1", "HEAD", typeof(SmtpConfigController), "GetNext")]
        [InlineData("/api/config/smtp-config/last", "HEAD", typeof(SmtpConfigController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/config/smtp-config/custom-fields", "HEAD", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/config/smtp-config/custom-fields", "HEAD", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/config/smtp-config/custom-fields/{resourceId}", "HEAD", typeof(SmtpConfigController), "GetCustomFields")]
        [InlineData("/api/config/smtp-config/custom-fields/{resourceId}", "HEAD", typeof(SmtpConfigController), "GetCustomFields")]

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

        public SmtpConfigRouteTests()
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