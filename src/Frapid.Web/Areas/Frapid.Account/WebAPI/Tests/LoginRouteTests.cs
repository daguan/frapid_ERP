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
    public class LoginRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/account/login/delete/{loginId}", "DELETE", typeof(LoginController), "Delete")]
        [InlineData("/api/account/login/delete/{loginId}", "DELETE", typeof(LoginController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/account/login/edit/{loginId}", "PUT", typeof(LoginController), "Edit")]
        [InlineData("/api/account/login/edit/{loginId}", "PUT", typeof(LoginController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/account/login/count-where", "POST", typeof(LoginController), "CountWhere")]
        [InlineData("/api/account/login/count-where", "POST", typeof(LoginController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/account/login/get-where/{pageNumber}", "POST", typeof(LoginController), "GetWhere")]
        [InlineData("/api/account/login/get-where/{pageNumber}", "POST", typeof(LoginController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/account/login/add-or-edit", "POST", typeof(LoginController), "AddOrEdit")]
        [InlineData("/api/account/login/add-or-edit", "POST", typeof(LoginController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/account/login/add/{login}", "POST", typeof(LoginController), "Add")]
        [InlineData("/api/account/login/add/{login}", "POST", typeof(LoginController), "Add")]
        [InlineData("/api/{apiVersionNumber}/account/login/bulk-import", "POST", typeof(LoginController), "BulkImport")]
        [InlineData("/api/account/login/bulk-import", "POST", typeof(LoginController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/account/login/meta", "GET", typeof(LoginController), "GetEntityView")]
        [InlineData("/api/account/login/meta", "GET", typeof(LoginController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/login/count", "GET", typeof(LoginController), "Count")]
        [InlineData("/api/account/login/count", "GET", typeof(LoginController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/login/all", "GET", typeof(LoginController), "GetAll")]
        [InlineData("/api/account/login/all", "GET", typeof(LoginController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/login/export", "GET", typeof(LoginController), "Export")]
        [InlineData("/api/account/login/export", "GET", typeof(LoginController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/login/1", "GET", typeof(LoginController), "Get")]
        [InlineData("/api/account/login/1", "GET", typeof(LoginController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/login/get?loginIds=1", "GET", typeof(LoginController), "Get")]
        [InlineData("/api/account/login/get?loginIds=1", "GET", typeof(LoginController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/login", "GET", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/account/login", "GET", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/login/page/1", "GET", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/account/login/page/1", "GET", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/login/count-filtered/{filterName}", "GET", typeof(LoginController), "CountFiltered")]
        [InlineData("/api/account/login/count-filtered/{filterName}", "GET", typeof(LoginController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/login/get-filtered/{pageNumber}/{filterName}", "GET", typeof(LoginController), "GetFiltered")]
        [InlineData("/api/account/login/get-filtered/{pageNumber}/{filterName}", "GET", typeof(LoginController), "GetFiltered")]
        [InlineData("/api/account/login/first", "GET", typeof(LoginController), "GetFirst")]
        [InlineData("/api/account/login/previous/1", "GET", typeof(LoginController), "GetPrevious")]
        [InlineData("/api/account/login/next/1", "GET", typeof(LoginController), "GetNext")]
        [InlineData("/api/account/login/last", "GET", typeof(LoginController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/login/custom-fields", "GET", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/account/login/custom-fields", "GET", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/login/custom-fields/{resourceId}", "GET", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/account/login/custom-fields/{resourceId}", "GET", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/login/meta", "HEAD", typeof(LoginController), "GetEntityView")]
        [InlineData("/api/account/login/meta", "HEAD", typeof(LoginController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/account/login/count", "HEAD", typeof(LoginController), "Count")]
        [InlineData("/api/account/login/count", "HEAD", typeof(LoginController), "Count")]
        [InlineData("/api/{apiVersionNumber}/account/login/all", "HEAD", typeof(LoginController), "GetAll")]
        [InlineData("/api/account/login/all", "HEAD", typeof(LoginController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/account/login/export", "HEAD", typeof(LoginController), "Export")]
        [InlineData("/api/account/login/export", "HEAD", typeof(LoginController), "Export")]
        [InlineData("/api/{apiVersionNumber}/account/login/1", "HEAD", typeof(LoginController), "Get")]
        [InlineData("/api/account/login/1", "HEAD", typeof(LoginController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/login/get?loginIds=1", "HEAD", typeof(LoginController), "Get")]
        [InlineData("/api/account/login/get?loginIds=1", "HEAD", typeof(LoginController), "Get")]
        [InlineData("/api/{apiVersionNumber}/account/login", "HEAD", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/account/login", "HEAD", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/login/page/1", "HEAD", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/account/login/page/1", "HEAD", typeof(LoginController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/account/login/count-filtered/{filterName}", "HEAD", typeof(LoginController), "CountFiltered")]
        [InlineData("/api/account/login/count-filtered/{filterName}", "HEAD", typeof(LoginController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/account/login/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(LoginController), "GetFiltered")]
        [InlineData("/api/account/login/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(LoginController), "GetFiltered")]
        [InlineData("/api/account/login/first", "HEAD", typeof(LoginController), "GetFirst")]
        [InlineData("/api/account/login/previous/1", "HEAD", typeof(LoginController), "GetPrevious")]
        [InlineData("/api/account/login/next/1", "HEAD", typeof(LoginController), "GetNext")]
        [InlineData("/api/account/login/last", "HEAD", typeof(LoginController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/account/login/custom-fields", "HEAD", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/account/login/custom-fields", "HEAD", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/account/login/custom-fields/{resourceId}", "HEAD", typeof(LoginController), "GetCustomFields")]
        [InlineData("/api/account/login/custom-fields/{resourceId}", "HEAD", typeof(LoginController), "GetCustomFields")]

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

        public LoginRouteTests()
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