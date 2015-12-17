// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.WebsiteBuilder.Api.Fakes;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class GetCategoryIdByCategoryAliasTests
    {
        public static GetCategoryIdByCategoryAliasController Fixture()
        {
            GetCategoryIdByCategoryAliasController controller = new GetCategoryIdByCategoryAliasController(new GetCategoryIdByCategoryAliasRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetCategoryIdByCategoryAliasController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}