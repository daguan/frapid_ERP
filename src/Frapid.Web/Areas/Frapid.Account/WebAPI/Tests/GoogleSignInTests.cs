// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GoogleSignInTests
    {
        public static GoogleSignInController Fixture()
        {
            GoogleSignInController controller = new GoogleSignInController(new GoogleSignInRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GoogleSignInController.Annotation());
            Assert.NotNull(actual);
        }
    }
}