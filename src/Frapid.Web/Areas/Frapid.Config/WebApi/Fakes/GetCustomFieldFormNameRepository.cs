// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class GetCustomFieldFormNameRepository : IGetCustomFieldFormNameRepository
    {
        public string TableName { get; set; }

        public string Execute()
        {
            return "FizzBuzz";
        }
    }
}