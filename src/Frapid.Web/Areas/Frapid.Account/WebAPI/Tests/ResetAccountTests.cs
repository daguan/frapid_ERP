// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class ResetAccountTests
    {
        public static ResetAccountController Fixture()
        {
            ResetAccountController controller = new ResetAccountController(new ResetAccountRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new ResetAccountController.Annotation());
            Assert.NotNull(actual);
        }
    }
}