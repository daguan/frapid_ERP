// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class FbSignInTests
    {
        public static FbSignInController Fixture()
        {
            FbSignInController controller = new FbSignInController(new FbSignInRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new FbSignInController.Annotation());
            Assert.NotNull(actual);
        }
    }
}