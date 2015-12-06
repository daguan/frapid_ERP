// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class GetCustomFieldDefinitionTests
    {
        public static GetCustomFieldDefinitionController Fixture()
        {
            GetCustomFieldDefinitionController controller = new GetCustomFieldDefinitionController(new GetCustomFieldDefinitionRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetCustomFieldDefinitionController.Annotation());
            Assert.NotNull(actual);
        }
    }
}