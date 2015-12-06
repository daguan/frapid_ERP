// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class CreateFlagTests
    {
        public static CreateFlagController Fixture()
        {
            CreateFlagController controller = new CreateFlagController(new CreateFlagRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            Fixture().Execute(new CreateFlagController.Annotation());
        }
    }
}