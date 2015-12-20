// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.WebsiteBuilder.Api.Fakes;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class AddEmailSubscriptionTests
    {
        public static AddEmailSubscriptionController Fixture()
        {
            AddEmailSubscriptionController controller = new AddEmailSubscriptionController(new AddEmailSubscriptionRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new AddEmailSubscriptionController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}