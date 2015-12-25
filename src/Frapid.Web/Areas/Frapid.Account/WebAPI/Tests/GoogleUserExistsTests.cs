// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class GoogleUserExistsTests
    {
        public static GoogleUserExistsController Fixture()
        {
            GoogleUserExistsController controller = new GoogleUserExistsController(new GoogleUserExistsRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GoogleUserExistsController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}