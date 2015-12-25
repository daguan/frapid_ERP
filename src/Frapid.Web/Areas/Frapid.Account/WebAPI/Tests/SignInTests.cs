// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class SignInTests
    {
        public static SignInController Fixture()
        {
            SignInController controller = new SignInController(new SignInRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new SignInController.Annotation());
            Assert.NotNull(actual);
        }
    }
}