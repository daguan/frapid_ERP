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
    public class ConfigurationProfileRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/delete/{profileId}", "DELETE", typeof(ConfigurationProfileController), "Delete")]
        [InlineData("/api/account/configuration-profile/delete/{profileId}", "DELETE", typeof(ConfigurationProfileController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/edit/{profileId}", "PUT", typeof(ConfigurationProfileController), "Edit")]
        [InlineData("/api/account/configuration-profile/edit/{profileId}", "PUT", typeof(ConfigurationProfileController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/count-where", "POST", typeof(ConfigurationProfileController), "CountWhere")]
        [InlineData("/api/account/configuration-profile/count-where", "POST", typeof(ConfigurationProfileController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/get-where/{pageNumber}", "POST", typeof(ConfigurationProfileController), "GetWhere")]
        [InlineData("/api/account/configuration-profile/get-where/{pageNumber}", "POST", typeof(ConfigurationProfileController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/add-or-edit", "POST", typeof(ConfigurationProfileController), "AddOrEdit")]
        [InlineData("/api/account/configuration-profile/add-or-edit", "POST", typeof(ConfigurationProfileController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/add/{configurationProfile}", "POST", typeof(ConfigurationProfileController), "Add")]
        [InlineData("/api/account/configuration-profile/add/{configurationProfile}", "POST", typeof(ConfigurationProfileController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/bulk-import", "POST", typeof(ConfigurationProfileController), "BulkImport")]
        [InlineData("/api/account/configuration-profile/bulk-import", "POST", typeof(ConfigurationProfileController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/meta", "GET", typeof(ConfigurationProfileController), "GetEntityView")]
        [InlineData("/api/account/configuration-profile/meta", "GET", typeof(ConfigurationProfileController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/count", "GET", typeof(ConfigurationProfileController), "Count")]
        [InlineData("/api/account/configuration-profile/count", "GET", typeof(ConfigurationProfileController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/all", "GET", typeof(ConfigurationProfileController), "GetAll")]
        [InlineData("/api/account/configuration-profile/all", "GET", typeof(ConfigurationProfileController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/export", "GET", typeof(ConfigurationProfileController), "Export")]
        [InlineData("/api/account/configuration-profile/export", "GET", typeof(ConfigurationProfileController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/1", "GET", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/account/configuration-profile/1", "GET", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/get?profileIds=1", "GET", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/account/configuration-profile/get?profileIds=1", "GET", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile", "GET", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/account/configuration-profile", "GET", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/page/1", "GET", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/account/configuration-profile/page/1", "GET", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/count-filtered/{filterName}", "GET", typeof(ConfigurationProfileController), "CountFiltered")]
        [InlineData("/api/account/configuration-profile/count-filtered/{filterName}", "GET", typeof(ConfigurationProfileController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ConfigurationProfileController), "GetFiltered")]
        [InlineData("/api/account/configuration-profile/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ConfigurationProfileController), "GetFiltered")]
        [InlineData("/api/account/configuration-profile/first", "GET", typeof(ConfigurationProfileController), "GetFirst")]
        [InlineData("/api/account/configuration-profile/previous/1", "GET", typeof(ConfigurationProfileController), "GetPrevious")]
        [InlineData("/api/account/configuration-profile/next/1", "GET", typeof(ConfigurationProfileController), "GetNext")]
        [InlineData("/api/account/configuration-profile/last", "GET", typeof(ConfigurationProfileController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/custom-fields", "GET", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/account/configuration-profile/custom-fields", "GET", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/custom-fields/{resourceId}", "GET", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/account/configuration-profile/custom-fields/{resourceId}", "GET", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/meta", "HEAD", typeof(ConfigurationProfileController), "GetEntityView")]
        [InlineData("/api/account/configuration-profile/meta", "HEAD", typeof(ConfigurationProfileController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/count", "HEAD", typeof(ConfigurationProfileController), "Count")]
        [InlineData("/api/account/configuration-profile/count", "HEAD", typeof(ConfigurationProfileController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/all", "HEAD", typeof(ConfigurationProfileController), "GetAll")]
        [InlineData("/api/account/configuration-profile/all", "HEAD", typeof(ConfigurationProfileController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/export", "HEAD", typeof(ConfigurationProfileController), "Export")]
        [InlineData("/api/account/configuration-profile/export", "HEAD", typeof(ConfigurationProfileController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/1", "HEAD", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/account/configuration-profile/1", "HEAD", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/get?profileIds=1", "HEAD", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/account/configuration-profile/get?profileIds=1", "HEAD", typeof(ConfigurationProfileController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile", "HEAD", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/account/configuration-profile", "HEAD", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/page/1", "HEAD", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/account/configuration-profile/page/1", "HEAD", typeof(ConfigurationProfileController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/count-filtered/{filterName}", "HEAD", typeof(ConfigurationProfileController), "CountFiltered")]
        [InlineData("/api/account/configuration-profile/count-filtered/{filterName}", "HEAD", typeof(ConfigurationProfileController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ConfigurationProfileController), "GetFiltered")]
        [InlineData("/api/account/configuration-profile/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ConfigurationProfileController), "GetFiltered")]
        [InlineData("/api/account/configuration-profile/first", "HEAD", typeof(ConfigurationProfileController), "GetFirst")]
        [InlineData("/api/account/configuration-profile/previous/1", "HEAD", typeof(ConfigurationProfileController), "GetPrevious")]
        [InlineData("/api/account/configuration-profile/next/1", "HEAD", typeof(ConfigurationProfileController), "GetNext")]
        [InlineData("/api/account/configuration-profile/last", "HEAD", typeof(ConfigurationProfileController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/custom-fields", "HEAD", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/account/configuration-profile/custom-fields", "HEAD", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/configuration-profile/custom-fields/{resourceId}", "HEAD", typeof(ConfigurationProfileController), "GetCustomFields")]
        [InlineData("/api/account/configuration-profile/custom-fields/{resourceId}", "HEAD", typeof(ConfigurationProfileController), "GetCustomFields")]

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

        public ConfigurationProfileRouteTests()
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