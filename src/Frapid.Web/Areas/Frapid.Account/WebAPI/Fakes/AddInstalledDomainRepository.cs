// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class AddInstalledDomainRepository : IAddInstalledDomainRepository
    {
        public string DomainName { get; set; }
        public string AdminEmail { get; set; }

        public void Execute()
        {
            return;
        }
    }
}