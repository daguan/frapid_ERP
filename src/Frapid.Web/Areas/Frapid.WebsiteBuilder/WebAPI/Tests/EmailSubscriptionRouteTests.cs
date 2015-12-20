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
    public class EmailSubscriptionRouteTests
    {
        [Theory]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/delete/{emailSubscriptionId}", "DELETE", typeof(EmailSubscriptionController), "Delete")]
        [InlineData("/api/website/email-subscription/delete/{emailSubscriptionId}", "DELETE", typeof(EmailSubscriptionController), "Delete")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/edit/{emailSubscriptionId}", "PUT", typeof(EmailSubscriptionController), "Edit")]
        [InlineData("/api/website/email-subscription/edit/{emailSubscriptionId}", "PUT", typeof(EmailSubscriptionController), "Edit")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/count-where", "POST", typeof(EmailSubscriptionController), "CountWhere")]
        [InlineData("/api/website/email-subscription/count-where", "POST", typeof(EmailSubscriptionController), "CountWhere")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/get-where/{pageNumber}", "POST", typeof(EmailSubscriptionController), "GetWhere")]
        [InlineData("/api/website/email-subscription/get-where/{pageNumber}", "POST", typeof(EmailSubscriptionController), "GetWhere")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/add-or-edit", "POST", typeof(EmailSubscriptionController), "AddOrEdit")]
        [InlineData("/api/website/email-subscription/add-or-edit", "POST", typeof(EmailSubscriptionController), "AddOrEdit")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/add/{emailSubscription}", "POST", typeof(EmailSubscriptionController), "Add")]
        [InlineData("/api/website/email-subscription/add/{emailSubscription}", "POST", typeof(EmailSubscriptionController), "Add")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/bulk-import", "POST", typeof(EmailSubscriptionController), "BulkImport")]
        [InlineData("/api/website/email-subscription/bulk-import", "POST", typeof(EmailSubscriptionController), "BulkImport")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/meta", "GET", typeof(EmailSubscriptionController), "GetEntityView")]
        [InlineData("/api/website/email-subscription/meta", "GET", typeof(EmailSubscriptionController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/count", "GET", typeof(EmailSubscriptionController), "Count")]
        [InlineData("/api/website/email-subscription/count", "GET", typeof(EmailSubscriptionController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/all", "GET", typeof(EmailSubscriptionController), "GetAll")]
        [InlineData("/api/website/email-subscription/all", "GET", typeof(EmailSubscriptionController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/export", "GET", typeof(EmailSubscriptionController), "Export")]
        [InlineData("/api/website/email-subscription/export", "GET", typeof(EmailSubscriptionController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/1", "GET", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/website/email-subscription/1", "GET", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/get?emailSubscriptionIds=1", "GET", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/website/email-subscription/get?emailSubscriptionIds=1", "GET", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription", "GET", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/website/email-subscription", "GET", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/page/1", "GET", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/website/email-subscription/page/1", "GET", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/count-filtered/{filterName}", "GET", typeof(EmailSubscriptionController), "CountFiltered")]
        [InlineData("/api/website/email-subscription/count-filtered/{filterName}", "GET", typeof(EmailSubscriptionController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/get-filtered/{pageNumber}/{filterName}", "GET", typeof(EmailSubscriptionController), "GetFiltered")]
        [InlineData("/api/website/email-subscription/get-filtered/{pageNumber}/{filterName}", "GET", typeof(EmailSubscriptionController), "GetFiltered")]
        [InlineData("/api/website/email-subscription/first", "GET", typeof(EmailSubscriptionController), "GetFirst")]
        [InlineData("/api/website/email-subscription/previous/1", "GET", typeof(EmailSubscriptionController), "GetPrevious")]
        [InlineData("/api/website/email-subscription/next/1", "GET", typeof(EmailSubscriptionController), "GetNext")]
        [InlineData("/api/website/email-subscription/last", "GET", typeof(EmailSubscriptionController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/email-subscription/custom-fields", "GET", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/website/email-subscription/custom-fields", "GET", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/custom-fields/{resourceId}", "GET", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/website/email-subscription/custom-fields/{resourceId}", "GET", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/meta", "HEAD", typeof(EmailSubscriptionController), "GetEntityView")]
        [InlineData("/api/website/email-subscription/meta", "HEAD", typeof(EmailSubscriptionController), "GetEntityView")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/count", "HEAD", typeof(EmailSubscriptionController), "Count")]
        [InlineData("/api/website/email-subscription/count", "HEAD", typeof(EmailSubscriptionController), "Count")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/all", "HEAD", typeof(EmailSubscriptionController), "GetAll")]
        [InlineData("/api/website/email-subscription/all", "HEAD", typeof(EmailSubscriptionController), "GetAll")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/export", "HEAD", typeof(EmailSubscriptionController), "Export")]
        [InlineData("/api/website/email-subscription/export", "HEAD", typeof(EmailSubscriptionController), "Export")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/1", "HEAD", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/website/email-subscription/1", "HEAD", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/get?emailSubscriptionIds=1", "HEAD", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/website/email-subscription/get?emailSubscriptionIds=1", "HEAD", typeof(EmailSubscriptionController), "Get")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription", "HEAD", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/website/email-subscription", "HEAD", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/page/1", "HEAD", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/website/email-subscription/page/1", "HEAD", typeof(EmailSubscriptionController), "GetPaginatedResult")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/count-filtered/{filterName}", "HEAD", typeof(EmailSubscriptionController), "CountFiltered")]
        [InlineData("/api/website/email-subscription/count-filtered/{filterName}", "HEAD", typeof(EmailSubscriptionController), "CountFiltered")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(EmailSubscriptionController), "GetFiltered")]
        [InlineData("/api/website/email-subscription/get-filtered/{pageNumber}/{filterName}", "HEAD", typeof(EmailSubscriptionController), "GetFiltered")]
        [InlineData("/api/website/email-subscription/first", "HEAD", typeof(EmailSubscriptionController), "GetFirst")]
        [InlineData("/api/website/email-subscription/previous/1", "HEAD", typeof(EmailSubscriptionController), "GetPrevious")]
        [InlineData("/api/website/email-subscription/next/1", "HEAD", typeof(EmailSubscriptionController), "GetNext")]
        [InlineData("/api/website/email-subscription/last", "HEAD", typeof(EmailSubscriptionController), "GetLast")]

        [InlineData("/api/{apiVersionNumber}/website/email-subscription/custom-fields", "HEAD", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/website/email-subscription/custom-fields", "HEAD", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/{apiVersionNumber}/website/email-subscription/custom-fields/{resourceId}", "HEAD", typeof(EmailSubscriptionController), "GetCustomFields")]
        [InlineData("/api/website/email-subscription/custom-fields/{resourceId}", "HEAD", typeof(EmailSubscriptionController), "GetCustomFields")]

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

        public EmailSubscriptionRouteTests()
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