// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class HasAccountTests
    {
        public static HasAccountController Fixture()
        {
            HasAccountController controller = new HasAccountController(new HasAccountRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new HasAccountController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}