// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class GetUserIdByLoginIdRepository : IGetUserIdByLoginIdRepository
    {
        public long LoginId { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}