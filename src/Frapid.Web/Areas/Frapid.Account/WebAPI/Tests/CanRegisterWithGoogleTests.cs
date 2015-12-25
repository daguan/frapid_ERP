// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class CanRegisterWithGoogleTests
    {
        public static CanRegisterWithGoogleController Fixture()
        {
            CanRegisterWithGoogleController controller = new CanRegisterWithGoogleController(new CanRegisterWithGoogleRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new CanRegisterWithGoogleController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}