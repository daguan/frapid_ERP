// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class ConfirmRegistrationTests
    {
        public static ConfirmRegistrationController Fixture()
        {
            ConfirmRegistrationController controller = new ConfirmRegistrationController(new ConfirmRegistrationRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new ConfirmRegistrationController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}