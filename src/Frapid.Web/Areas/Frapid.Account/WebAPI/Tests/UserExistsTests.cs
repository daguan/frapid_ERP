// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class UserExistsTests
    {
        public static UserExistsController Fixture()
        {
            UserExistsController controller = new UserExistsController(new UserExistsRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new UserExistsController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}