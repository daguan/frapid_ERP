// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class GetFlagTypeIdRepository : IGetFlagTypeIdRepository
    {
        public int UserId { get; set; }
        public string Resource { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceId { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}