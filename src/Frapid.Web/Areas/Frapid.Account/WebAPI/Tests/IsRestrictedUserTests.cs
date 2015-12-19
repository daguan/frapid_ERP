// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class IsRestrictedUserTests
    {
        public static IsRestrictedUserController Fixture()
        {
            IsRestrictedUserController controller = new IsRestrictedUserController(new IsRestrictedUserRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new IsRestrictedUserController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}