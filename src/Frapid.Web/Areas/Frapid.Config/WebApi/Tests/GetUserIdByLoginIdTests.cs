// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class GetUserIdByLoginIdTests
    {
        public static GetUserIdByLoginIdController Fixture()
        {
            GetUserIdByLoginIdController controller = new GetUserIdByLoginIdController(new GetUserIdByLoginIdRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetUserIdByLoginIdController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}