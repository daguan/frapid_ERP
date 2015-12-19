// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class CompleteResetRepository : ICompleteResetRepository
    {
        public System.Guid RequestId { get; set; }
        public string Password { get; set; }

        public void Execute()
        {
            return;
        }
    }
}