// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.WebsiteBuilder.Api.Fakes;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class GetCategoryIdByCategoryNameTests
    {
        public static GetCategoryIdByCategoryNameController Fixture()
        {
            GetCategoryIdByCategoryNameController controller = new GetCategoryIdByCategoryNameController(new GetCategoryIdByCategoryNameRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetCategoryIdByCategoryNameController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}