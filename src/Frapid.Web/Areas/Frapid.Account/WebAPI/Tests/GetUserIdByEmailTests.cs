// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GetUserIdByEmailTests
    {
        public static GetUserIdByEmailController Fixture()
        {
            GetUserIdByEmailController controller = new GetUserIdByEmailController(new GetUserIdByEmailRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetUserIdByEmailController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}