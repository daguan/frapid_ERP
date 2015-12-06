// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class GetCustomFieldFormNameTests
    {
        public static GetCustomFieldFormNameController Fixture()
        {
            GetCustomFieldFormNameController controller = new GetCustomFieldFormNameController(new GetCustomFieldFormNameRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetCustomFieldFormNameController.Annotation());
            Assert.Equal("FizzBuzz", actual);
        }
    }
}