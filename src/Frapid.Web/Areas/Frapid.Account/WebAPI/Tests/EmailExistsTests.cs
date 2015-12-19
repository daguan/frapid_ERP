// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class EmailExistsTests
    {
        public static EmailExistsController Fixture()
        {
            EmailExistsController controller = new EmailExistsController(new EmailExistsRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new EmailExistsController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}