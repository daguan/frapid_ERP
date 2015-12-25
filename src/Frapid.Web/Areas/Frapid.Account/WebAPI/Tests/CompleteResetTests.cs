// ReSharper disable All
using System;
using System.Diagnostics;
using System.Linq;
using Frapid.ApplicationState.Models;
using Frapid.Account.Api.Fakes;
using Xunit;

namespace Frapid.Account.Api.Tests
{
    public class CompleteResetTests
    {
        public static CompleteResetController Fixture()
        {
            CompleteResetController controller = new CompleteResetController(new CompleteResetRepository());
            return controller;
        }

        [Fact]
        [Conditional("Debug")]
        public void Execute()
        {
            Fixture().Execute(new CompleteResetController.Annotation());
        }
    }
}