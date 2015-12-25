// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class FbUserExistsTests
    {
        public static FbUserExistsController Fixture()
        {
            FbUserExistsController controller = new FbUserExistsController(new FbUserExistsRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new FbUserExistsController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}