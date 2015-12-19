// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class HasAccountRepository : IHasAccountRepository
    {
        public string Email { get; set; }

        public bool Execute()
        {
            return new bool();
        }
    }
}