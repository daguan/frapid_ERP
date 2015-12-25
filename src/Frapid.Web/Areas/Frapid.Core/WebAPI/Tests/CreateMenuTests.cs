// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Core.Api.Fakes;
using Xunit;

namespace Frapid.Core.Api.Tests
{
    public class CreateMenuTests
    {
        public static CreateMenuController Fixture()
        {
            CreateMenuController controller = new CreateMenuController(new CreateMenuRepository());
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