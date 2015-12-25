// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class IsValidClientTokenTests
    {
        public static IsValidClientTokenController Fixture()
        {
            IsValidClientTokenController controller = new IsValidClientTokenController(new IsValidClientTokenRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new IsValidClientTokenController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}