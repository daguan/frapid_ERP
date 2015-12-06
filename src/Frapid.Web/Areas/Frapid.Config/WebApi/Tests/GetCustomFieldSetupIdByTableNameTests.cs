// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Config.Api.Fakes;
using Xunit;

namespace Frapid.Config.Api.Tests
{
    public class GetCustomFieldSetupIdByTableNameTests
    {
        public static GetCustomFieldSetupIdByTableNameController Fixture()
        {
            GetCustomFieldSetupIdByTableNameController controller = new GetCustomFieldSetupIdByTableNameController(new GetCustomFieldSetupIdByTableNameRepository(), "", new LoginView());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetCustomFieldSetupIdByTableNameController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}