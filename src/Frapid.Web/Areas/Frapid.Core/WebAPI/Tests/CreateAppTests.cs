// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Core.Api.Fakes;
using Xunit;

namespace Frapid.Core.Api.Tests
{
    public class CreateAppTests
    {
        public static CreateAppController Fixture()
        {
            CreateAppController controller = new CreateAppController(new CreateAppRepository());
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