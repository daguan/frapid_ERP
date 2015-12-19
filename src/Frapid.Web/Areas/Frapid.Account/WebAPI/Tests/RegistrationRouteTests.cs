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
    public class RegistrationRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/registration/delete/{registrationId}", "DELETE", typeof(RegistrationController), "Delete")]
        [InlineData("/api/account/registration/delete/{registrationId}", "DELETE", typeof(RegistrationController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/registration/edit/{registrationId}", "PUT", typeof(RegistrationController), "Edit")]
        [InlineData("/api/account/registration/edit/{registrationId}", "PUT", typeof(RegistrationController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/registration/count-where", "POST", typeof(RegistrationController), "CountWhere")]
        [InlineData("/api/account/registration/count-where", "POST", typeof(RegistrationController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/registration/get-where/{pageNumber}", "POST", typeof(RegistrationController), "GetWhere")]
        [InlineData("/api/account/registration/get-where/{pageNumber}", "POST", typeof(RegistrationController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/registration/add-or-edit", "POST", typeof(RegistrationController), "AddOrEdit")]
        [InlineData("/api/account/registration/add-or-edit", "POST", typeof(RegistrationController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/registration/add/{registration}", "POST", typeof(RegistrationController), "Add")]
        [InlineData("/api/account/registration/add/{registration}", "POST", typeof(RegistrationController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/registration/bulk-import", "POST", typeof(RegistrationController), "BulkImport")]
        [InlineData("/api/account/registration/bulk-import", "POST", typeof(RegistrationController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/registration/meta", "GET", typeof(RegistrationController), "GetEntityView")]
        [InlineData("/api/account/registration/meta", "GET", typeof(RegistrationController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/registration/count", "GET", typeof(RegistrationController), "Count")]
        [InlineData("/api/account/registration/count", "GET", typeof(RegistrationController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/registration/all", "GET", typeof(RegistrationController), "GetAll")]
        [InlineData("/api/account/registration/all", "GET", typeof(RegistrationController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/registration/export", "GET", typeof(RegistrationController), "Export")]
        [InlineData("/api/account/registration/export", "GET", typeof(RegistrationController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/registration/1", "GET", typeof(RegistrationController), "Get")]
        [InlineData("/api/account/registration/1", "GET", typeof(RegistrationController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/registration/get?registrationIds=1", "GET", typeof(RegistrationController), "Get")]
        [InlineData("/api/account/registration/get?registrationIds=1", "GET", typeof(RegistrationController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/registration", "GET", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/account/registration", "GET", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/registration/page/1", "GET", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/account/registration/page/1", "GET", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/registration/count-filtered/{filterName}", "GET", typeof(RegistrationController), "CountFiltered")]
        [InlineData("/api/account/registration/count-filtered/{filterName}", "GET", typeof(RegistrationController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/registration/get-filtered/{pageNumber}/{filterName}", "GET", typeof(RegistrationController), "GetFiltered")]
        [InlineData("/api/account/registration/get-filtered/{pageNumber}/{filterName}", "GET", typeof(RegistrationController), "GetFiltered")]
        [InlineData("/api/account/registration/first", "GET", typeof(RegistrationController), "GetFirst")]
        [InlineData("/api/account/registration/previous/1", "GET", typeof(RegistrationController), "GetPrevious")]
        [InlineData("/api/account/registration/next/1", "GET", typeof(RegistrationController), "GetNext")]
        [InlineData("/api/account/registration/last", "GET", typeof(RegistrationController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/registration/custom-fields", "GET", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/account/registration/custom-fields", "GET", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/registration/custom-fields/{resourceId}", "GET", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/account/registration/custom-fields/{resourceId}", "GET", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/registration/meta", "HEAD", typeof(RegistrationController), "GetEntityView")]
        [InlineData("/api/account/registration/meta", "HEAD", typeof(RegistrationController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/registration/count", "HEAD", typeof(RegistrationController), "Count")]
        [InlineData("/api/account/registration/count", "HEAD", typeof(RegistrationController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/registration/all", "HEAD", typeof(RegistrationController), "GetAll")]
        [InlineData("/api/account/registration/all", "HEAD", typeof(RegistrationController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/registration/export", "HEAD", typeof(RegistrationController), "Export")]
        [InlineData("/api/account/registration/export", "HEAD", typeof(RegistrationController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/registration/1", "HEAD", typeof(RegistrationController), "Get")]
        [InlineData("/api/account/registration/1", "HEAD", typeof(RegistrationController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/registration/get?registrationIds=1", "HEAD", typeof(RegistrationController), "Get")]
        [InlineData("/api/account/registration/get?registrationIds=1", "HEAD", typeof(RegistrationController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/registration", "HEAD", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/account/registration", "HEAD", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/registration/page/1", "HEAD", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/account/registration/page/1", "HEAD", typeof(RegistrationController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/registration/count-filtered/{filterName}", "HEAD", typeof(RegistrationController), "CountFiltered")]
        [InlineData("/api/account/registration/count-filtered/{filterName}", "HEAD", typeof(RegistrationController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/registration/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(RegistrationController), "GetFiltered")]
        [InlineData("/api/account/registration/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(RegistrationController), "GetFiltered")]
        [InlineData("/api/account/registration/first", "HEAD", typeof(RegistrationController), "GetFirst")]
        [InlineData("/api/account/registration/previous/1", "HEAD", typeof(RegistrationController), "GetPrevious")]
        [InlineData("/api/account/registration/next/1", "HEAD", typeof(RegistrationController), "GetNext")]
        [InlineData("/api/account/registration/last", "HEAD", typeof(RegistrationController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/registration/custom-fields", "HEAD", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/account/registration/custom-fields", "HEAD", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/registration/custom-fields/{resourceId}", "HEAD", typeof(RegistrationController), "GetCustomFields")]
        [InlineData("/api/account/registration/custom-fields/{resourceId}", "HEAD", typeof(RegistrationController), "GetCustomFields")]

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

        public RegistrationRouteTests()
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