// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class CreateFlagRepository : ICreateFlagRepository
    {
        public int UserId { get; set; }
        public int FlagTypeId { get; set; }
        public string Resource { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceId { get; set; }

        public void Execute()
        {
            return;
        }
    }
}