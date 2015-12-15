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
    public class ContactRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/contact/delete/{contactId}", "DELETE", typeof(ContactController), "Delete")]
        [InlineData("/api/website/contact/delete/{contactId}", "DELETE", typeof(ContactController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/contact/edit/{contactId}", "PUT", typeof(ContactController), "Edit")]
        [InlineData("/api/website/contact/edit/{contactId}", "PUT", typeof(ContactController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/contact/count-where", "POST", typeof(ContactController), "CountWhere")]
        [InlineData("/api/website/contact/count-where", "POST", typeof(ContactController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/contact/get-where/{pageNumber}", "POST", typeof(ContactController), "GetWhere")]
        [InlineData("/api/website/contact/get-where/{pageNumber}", "POST", typeof(ContactController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/contact/add-or-edit", "POST", typeof(ContactController), "AddOrEdit")]
        [InlineData("/api/website/contact/add-or-edit", "POST", typeof(ContactController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/contact/add/{contact}", "POST", typeof(ContactController), "Add")]
        [InlineData("/api/website/contact/add/{contact}", "POST", typeof(ContactController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/contact/bulk-import", "POST", typeof(ContactController), "BulkImport")]
        [InlineData("/api/website/contact/bulk-import", "POST", typeof(ContactController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/contact/meta", "GET", typeof(ContactController), "GetEntityView")]
        [InlineData("/api/website/contact/meta", "GET", typeof(ContactController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/contact/count", "GET", typeof(ContactController), "Count")]
        [InlineData("/api/website/contact/count", "GET", typeof(ContactController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/contact/all", "GET", typeof(ContactController), "GetAll")]
        [InlineData("/api/website/contact/all", "GET", typeof(ContactController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/contact/export", "GET", typeof(ContactController), "Export")]
        [InlineData("/api/website/contact/export", "GET", typeof(ContactController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/contact/1", "GET", typeof(ContactController), "Get")]
        [InlineData("/api/website/contact/1", "GET", typeof(ContactController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/contact/get?contactIds=1", "GET", typeof(ContactController), "Get")]
        [InlineData("/api/website/contact/get?contactIds=1", "GET", typeof(ContactController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/contact", "GET", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/website/contact", "GET", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/contact/page/1", "GET", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/website/contact/page/1", "GET", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/contact/count-filtered/{filterName}", "GET", typeof(ContactController), "CountFiltered")]
        [InlineData("/api/website/contact/count-filtered/{filterName}", "GET", typeof(ContactController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/contact/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ContactController), "GetFiltered")]
        [InlineData("/api/website/contact/get-filtered/{pageNumber}/{filterName}", "GET", typeof(ContactController), "GetFiltered")]
        [InlineData("/api/website/contact/first", "GET", typeof(ContactController), "GetFirst")]
        [InlineData("/api/website/contact/previous/1", "GET", typeof(ContactController), "GetPrevious")]
        [InlineData("/api/website/contact/next/1", "GET", typeof(ContactController), "GetNext")]
        [InlineData("/api/website/contact/last", "GET", typeof(ContactController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/contact/custom-fields", "GET", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/website/contact/custom-fields", "GET", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/contact/custom-fields/{resourceId}", "GET", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/website/contact/custom-fields/{resourceId}", "GET", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/contact/meta", "HEAD", typeof(ContactController), "GetEntityView")]
        [InlineData("/api/website/contact/meta", "HEAD", typeof(ContactController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/contact/count", "HEAD", typeof(ContactController), "Count")]
        [InlineData("/api/website/contact/count", "HEAD", typeof(ContactController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/contact/all", "HEAD", typeof(ContactController), "GetAll")]
        [InlineData("/api/website/contact/all", "HEAD", typeof(ContactController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/contact/export", "HEAD", typeof(ContactController), "Export")]
        [InlineData("/api/website/contact/export", "HEAD", typeof(ContactController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/contact/1", "HEAD", typeof(ContactController), "Get")]
        [InlineData("/api/website/contact/1", "HEAD", typeof(ContactController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/contact/get?contactIds=1", "HEAD", typeof(ContactController), "Get")]
        [InlineData("/api/website/contact/get?contactIds=1", "HEAD", typeof(ContactController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/contact", "HEAD", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/website/contact", "HEAD", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/contact/page/1", "HEAD", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/website/contact/page/1", "HEAD", typeof(ContactController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/contact/count-filtered/{filterName}", "HEAD", typeof(ContactController), "CountFiltered")]
        [InlineData("/api/website/contact/count-filtered/{filterName}", "HEAD", typeof(ContactController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/contact/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ContactController), "GetFiltered")]
        [InlineData("/api/website/contact/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(ContactController), "GetFiltered")]
        [InlineData("/api/website/contact/first", "HEAD", typeof(ContactController), "GetFirst")]
        [InlineData("/api/website/contact/previous/1", "HEAD", typeof(ContactController), "GetPrevious")]
        [InlineData("/api/website/contact/next/1", "HEAD", typeof(ContactController), "GetNext")]
        [InlineData("/api/website/contact/last", "HEAD", typeof(ContactController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/contact/custom-fields", "HEAD", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/website/contact/custom-fields", "HEAD", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/contact/custom-fields/{resourceId}", "HEAD", typeof(ContactController), "GetCustomFields")]
        [InlineData("/api/website/contact/custom-fields/{resourceId}", "HEAD", typeof(ContactController), "GetCustomFields")]

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

        public ContactRouteTests()
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