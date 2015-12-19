// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GetRegistrationRoleIdTests
    {
        public static GetRegistrationRoleIdController Fixture()
        {
            GetRegistrationRoleIdController controller = new GetRegistrationRoleIdController(new GetRegistrationRoleIdRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetRegistrationRoleIdController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}