// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GetRegistrationOfficeIdTests
    {
        public static GetRegistrationOfficeIdController Fixture()
        {
            GetRegistrationOfficeIdController controller = new GetRegistrationOfficeIdController(new GetRegistrationOfficeIdRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetRegistrationOfficeIdController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}