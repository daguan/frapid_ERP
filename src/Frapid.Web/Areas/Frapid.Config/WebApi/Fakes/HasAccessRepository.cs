// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class HasAccessRepository : IHasAccessRepository
    {
        public int UserId { get; set; }
        public string Entity { get; set; }
        public int AccessTypeId { get; set; }

        public bool Execute()
        {
            return new bool();
        }
    }
}