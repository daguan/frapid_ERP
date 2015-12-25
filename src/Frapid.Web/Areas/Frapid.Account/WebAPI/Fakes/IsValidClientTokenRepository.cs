// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class IsValidClientTokenRepository : IIsValidClientTokenRepository
    {
        public string ClientToken { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

        public bool Execute()
        {
            return new bool();
        }
    }
}