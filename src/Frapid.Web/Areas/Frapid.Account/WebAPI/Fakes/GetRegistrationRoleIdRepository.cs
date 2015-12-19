// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Account.DataAccess;
using Frapid.Account.Entities;

namespace Frapid.Account.Api.Fakes
{
    public class GetRegistrationRoleIdRepository : IGetRegistrationRoleIdRepository
    {

        public int Execute()
        {
            return 1;
        }
    }
}