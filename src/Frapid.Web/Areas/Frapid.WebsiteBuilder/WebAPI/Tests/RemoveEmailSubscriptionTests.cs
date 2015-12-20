// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.WebsiteBuilder.Api.Fakes;
using Xunit;

namespace Frapid.WebsiteBuilder.Api.Tests
{
    public class RemoveEmailSubscriptionTests
    {
        public static RemoveEmailSubscriptionController Fixture()
        {
            RemoveEmailSubscriptionController controller = new RemoveEmailSubscriptionController(new RemoveEmailSubscriptionRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new RemoveEmailSubscriptionController.Annotation());
            Assert.Equal(new bool(), actual);
        }
    }
}