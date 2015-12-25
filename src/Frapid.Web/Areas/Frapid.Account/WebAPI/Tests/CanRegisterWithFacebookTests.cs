// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class CanRegisterWithFacebookTests
    {
        public static CanRegisterWithFacebookController Fixture()
        {
            CanRegisterWithFacebookController controller = new CanRegisterWithFacebookController(new CanRegisterWithFacebookRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new CanRegisterWithFacebookController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}