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
    public class InstalledDomainRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/delete/{domainId}", "DELETE", typeof(InstalledDomainController), "Delete")]
        [InlineData("/api/account/installed-domain/delete/{domainId}", "DELETE", typeof(InstalledDomainController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/edit/{domainId}", "PUT", typeof(InstalledDomainController), "Edit")]
        [InlineData("/api/account/installed-domain/edit/{domainId}", "PUT", typeof(InstalledDomainController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/count-where", "POST", typeof(InstalledDomainController), "CountWhere")]
        [InlineData("/api/account/installed-domain/count-where", "POST", typeof(InstalledDomainController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/get-where/{pageNumber}", "POST", typeof(InstalledDomainController), "GetWhere")]
        [InlineData("/api/account/installed-domain/get-where/{pageNumber}", "POST", typeof(InstalledDomainController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/add-or-edit", "POST", typeof(InstalledDomainController), "AddOrEdit")]
        [InlineData("/api/account/installed-domain/add-or-edit", "POST", typeof(InstalledDomainController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/add/{installedDomain}", "POST", typeof(InstalledDomainController), "Add")]
        [InlineData("/api/account/installed-domain/add/{installedDomain}", "POST", typeof(InstalledDomainController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/bulk-import", "POST", typeof(InstalledDomainController), "BulkImport")]
        [InlineData("/api/account/installed-domain/bulk-import", "POST", typeof(InstalledDomainController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/meta", "GET", typeof(InstalledDomainController), "GetEntityView")]
        [InlineData("/api/account/installed-domain/meta", "GET", typeof(InstalledDomainController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/count", "GET", typeof(InstalledDomainController), "Count")]
        [InlineData("/api/account/installed-domain/count", "GET", typeof(InstalledDomainController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/all", "GET", typeof(InstalledDomainController), "GetAll")]
        [InlineData("/api/account/installed-domain/all", "GET", typeof(InstalledDomainController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/export", "GET", typeof(InstalledDomainController), "Export")]
        [InlineData("/api/account/installed-domain/export", "GET", typeof(InstalledDomainController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/1", "GET", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/account/installed-domain/1", "GET", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/get?domainIds=1", "GET", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/account/installed-domain/get?domainIds=1", "GET", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain", "GET", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/account/installed-domain", "GET", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/page/1", "GET", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/account/installed-domain/page/1", "GET", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/count-filtered/{filterName}", "GET", typeof(InstalledDomainController), "CountFiltered")]
        [InlineData("/api/account/installed-domain/count-filtered/{filterName}", "GET", typeof(InstalledDomainController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/get-filtered/{pageNumber}/{filterName}", "GET", typeof(InstalledDomainController), "GetFiltered")]
        [InlineData("/api/account/installed-domain/get-filtered/{pageNumber}/{filterName}", "GET", typeof(InstalledDomainController), "GetFiltered")]
        [InlineData("/api/account/installed-domain/first", "GET", typeof(InstalledDomainController), "GetFirst")]
        [InlineData("/api/account/installed-domain/previous/1", "GET", typeof(InstalledDomainController), "GetPrevious")]
        [InlineData("/api/account/installed-domain/next/1", "GET", typeof(InstalledDomainController), "GetNext")]
        [InlineData("/api/account/installed-domain/last", "GET", typeof(InstalledDomainController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/installed-domain/custom-fields", "GET", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/account/installed-domain/custom-fields", "GET", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/custom-fields/{resourceId}", "GET", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/account/installed-domain/custom-fields/{resourceId}", "GET", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/meta", "HEAD", typeof(InstalledDomainController), "GetEntityView")]
        [InlineData("/api/account/installed-domain/meta", "HEAD", typeof(InstalledDomainController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/count", "HEAD", typeof(InstalledDomainController), "Count")]
        [InlineData("/api/account/installed-domain/count", "HEAD", typeof(InstalledDomainController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/all", "HEAD", typeof(InstalledDomainController), "GetAll")]
        [InlineData("/api/account/installed-domain/all", "HEAD", typeof(InstalledDomainController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/export", "HEAD", typeof(InstalledDomainController), "Export")]
        [InlineData("/api/account/installed-domain/export", "HEAD", typeof(InstalledDomainController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/1", "HEAD", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/account/installed-domain/1", "HEAD", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/get?domainIds=1", "HEAD", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/account/installed-domain/get?domainIds=1", "HEAD", typeof(InstalledDomainController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain", "HEAD", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/account/installed-domain", "HEAD", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/page/1", "HEAD", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/account/installed-domain/page/1", "HEAD", typeof(InstalledDomainController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/count-filtered/{filterName}", "HEAD", typeof(InstalledDomainController), "CountFiltered")]
        [InlineData("/api/account/installed-domain/count-filtered/{filterName}", "HEAD", typeof(InstalledDomainController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(InstalledDomainController), "GetFiltered")]
        [InlineData("/api/account/installed-domain/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(InstalledDomainController), "GetFiltered")]
        [InlineData("/api/account/installed-domain/first", "HEAD", typeof(InstalledDomainController), "GetFirst")]
        [InlineData("/api/account/installed-domain/previous/1", "HEAD", typeof(InstalledDomainController), "GetPrevious")]
        [InlineData("/api/account/installed-domain/next/1", "HEAD", typeof(InstalledDomainController), "GetNext")]
        [InlineData("/api/account/installed-domain/last", "HEAD", typeof(InstalledDomainController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/installed-domain/custom-fields", "HEAD", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/account/installed-domain/custom-fields", "HEAD", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/installed-domain/custom-fields/{resourceId}", "HEAD", typeof(InstalledDomainController), "GetCustomFields")]
        [InlineData("/api/account/installed-domain/custom-fields/{resourceId}", "HEAD", typeof(InstalledDomainController), "GetCustomFields")]

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

        public InstalledDomainRouteTests()
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