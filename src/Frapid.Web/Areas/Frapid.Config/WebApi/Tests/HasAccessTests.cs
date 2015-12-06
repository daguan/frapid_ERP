// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class HasAccessTests
    {
        public static HasAccessController Fixture()
        {
            HasAccessController controller = new HasAccessController(new HasAccessRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new HasAccessController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}