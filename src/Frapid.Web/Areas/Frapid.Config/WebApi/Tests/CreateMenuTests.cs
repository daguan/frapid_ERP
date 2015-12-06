// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class CreateMenuTests
    {
        public static CreateMenuController Fixture()
        {
            CreateMenuController controller = new CreateMenuController(new CreateMenuRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new CreateMenuController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}