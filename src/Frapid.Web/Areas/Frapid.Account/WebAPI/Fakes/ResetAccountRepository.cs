// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class ResetAccountRepository : IResetAccountRepository
    {
        public string Email { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }

        public IEnumerable<DbResetAccountResult> Execute()
        {
            return new List<DbResetAccountResult>();
        }
    }
}