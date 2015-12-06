// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class CreateAppTests
    {
        public static CreateAppController Fixture()
        {
            CreateAppController controller = new CreateAppController(new CreateAppRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            Fixture().Execute(new CreateAppController.Annotation());
        }
    }
}