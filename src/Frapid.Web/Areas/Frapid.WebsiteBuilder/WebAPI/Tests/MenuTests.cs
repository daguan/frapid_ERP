// ReSharper disable All
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Frapid.ApplicationState.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.WebsiteBuilder.Api.Fakes;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class MenuTests
    {
        public static WebsiteMenuController Fixture()
        {
            WebsiteMenuController controller = new WebsiteMenuController(new MenuRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void CountEntityColumns()
        {
            EntityView entityView = Fixture().GetEntityView();
            Assert.Null(entityView.Columns);
        }

        [Fact]
        [Conditional("Debug")]
        public void Count()
        {
            long count = Fixture().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetAll()
        {
            int count = Fixture().GetAll().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void Export()
        {
            int count = Fixture().Export().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void Get()
        {
            Frapid.WebsiteBuilder.Entities.Menu menu = Fixture().Get(0);
            Assert.NotNull(menu);
        }

        [Fact]
        [Conditional("Debug")]
        public void First()
        {
            Frapid.WebsiteBuilder.Entities.Menu menu = Fixture().GetFirst();
            Assert.NotNull(menu);
        }

        [Fact]
        [Conditional("Debug")]
        public void Previous()
        {
            Frapid.WebsiteBuilder.Entities.Menu menu = Fixture().GetPrevious(0);
            Assert.NotNull(menu);
        }

        [Fact]
        [Conditional("Debug")]
        public void Next()
        {
            Frapid.WebsiteBuilder.Entities.Menu menu = Fixture().GetNext(0);
            Assert.NotNull(menu);
        }

        [Fact]
        [Conditional("Debug")]
        public void Last()
        {
            Frapid.WebsiteBuilder.Entities.Menu menu = Fixture().GetLast();
            Assert.NotNull(menu);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetMultiple()
        {
            IEnumerable<Frapid.WebsiteBuilder.Entities.Menu> menus = Fixture().Get(new int[] { });
            Assert.NotNull(menus);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetPaginatedResult()
        {
            int count = Fixture().GetPaginatedResult().Count();
            Assert.Equal(1, count);

            count = Fixture().GetPaginatedResult(1).Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void CountWhere()
        {
            long count = Fixture().CountWhere(new JArray());
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetWhere()
        {
            int count = Fixture().GetWhere(1, new JArray()).Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void CountFiltered()
        {
            long count = Fixture().CountFiltered("");
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetFiltered()
        {
            int count = Fixture().GetFiltered(1, "").Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetDisplayFields()
        {
            int count = Fixture().GetDisplayFields().Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetCustomFields()
        {
            int count = Fixture().GetCustomFields().Count();
            Assert.Equal(1, count);

            count = Fixture().GetCustomFields("").Count();
            Assert.Equal(1, count);
        }

        [Fact]
        [Conditional("Debug")]
        public void AddOrEdit()
        {
            try
            {
                var form = new JArray { null, null };
                Fixture().AddOrEdit(form);
            }
            catch (HttpResponseException ex)
            {
                Assert.Equal(HttpStatusCode.MethodNotAllowed, ex.Response.StatusCode);
            }
        }


        [Fact]
        [Conditional("Debug")]
        public void Add()
        {
            try
            {
                Fixture().Add(null);
            }
            catch (HttpResponseException ex)
            {
                Assert.Equal(HttpStatusCode.MethodNotAllowed, ex.Response.StatusCode);
            }
        }

        [Fact]
        [Conditional("Debug")]
        public void Edit()
        {
            try
            {
                Fixture().Edit(0, null);
            }
            catch (HttpResponseException ex)
            {
                Assert.Equal(HttpStatusCode.MethodNotAllowed, ex.Response.StatusCode);
            }
        }

        [Fact]
        [Conditional("Debug")]
        public void BulkImport()
        {
            var collection = new JArray { null, null, null, null };
            var actual = Fixture().BulkImport(collection);

            Assert.NotNull(actual);
        }

        [Fact]
        [Conditional("Debug")]
        public void Delete()
        {
            try
            {
                Fixture().Delete(0);
            }
            catch (HttpResponseException ex)
            {
                Assert.Equal(HttpStatusCode.InternalServerError, ex.Response.StatusCode);
            }
        }


    }
}