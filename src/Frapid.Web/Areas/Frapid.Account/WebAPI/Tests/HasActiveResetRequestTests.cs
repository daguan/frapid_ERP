// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class HasActiveResetRequestTests
    {
        public static HasActiveResetRequestController Fixture()
        {
            HasActiveResetRequestController controller = new HasActiveResetRequestController(new HasActiveResetRequestRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new HasActiveResetRequestController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}