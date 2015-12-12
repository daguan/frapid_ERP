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
using Frapid.Config.DataAccess;
using Frapid.Config.Api.Fakes;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class CustomFieldSetupTests
    {
        public static CustomFieldSetupController Fixture()
        {
            CustomFieldSetupController controller = new CustomFieldSetupController(new CustomFieldSetupRepository(), "", new LoginView());
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
            Frapid.Config.Entities.CustomFieldSetup customFieldSetup = Fixture().Get(0);
            Assert.NotNull(customFieldSetup);
        }

        [Fact]
        [Conditional("Debug")]
        public void First()
        {
            Frapid.Config.Entities.CustomFieldSetup customFieldSetup = Fixture().GetFirst();
            Assert.NotNull(customFieldSetup);
        }

        [Fact]
        [Conditional("Debug")]
        public void Previous()
        {
            Frapid.Config.Entities.CustomFieldSetup customFieldSetup = Fixture().GetPrevious(0);
            Assert.NotNull(customFieldSetup);
        }

        [Fact]
        [Conditional("Debug")]
        public void Next()
        {
            Frapid.Config.Entities.CustomFieldSetup customFieldSetup = Fixture().GetNext(0);
            Assert.NotNull(customFieldSetup);
        }

        [Fact]
        [Conditional("Debug")]
        public void Last()
        {
            Frapid.Config.Entities.CustomFieldSetup customFieldSetup = Fixture().GetLast();
            Assert.NotNull(customFieldSetup);
        }

        [Fact]
        [Conditional("Debug")]
        public void GetMultiple()
        {
            IEnumerable<Frapid.Config.Entities.CustomFieldSetup> customFieldSetups = Fixture().Get(new int[] { });
            Assert.NotNull(customFieldSetups);
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