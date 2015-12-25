// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Core.Api.Fakes;
using Xunit;

namespace Frapid.Core.Api.Tests
{
    public class GetOfficeIdByOfficeNameTests
    {
        public static GetOfficeIdByOfficeNameController Fixture()
        {
            GetOfficeIdByOfficeNameController controller = new GetOfficeIdByOfficeNameController(new GetOfficeIdByOfficeNameRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            var actual = Fixture().Execute(new GetOfficeIdByOfficeNameController.Annotation());
            Assert.Equal(1, actual);
        }
    }
}