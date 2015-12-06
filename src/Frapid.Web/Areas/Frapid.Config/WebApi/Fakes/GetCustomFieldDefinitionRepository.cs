// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class GetCustomFieldDefinitionRepository : IGetCustomFieldDefinitionRepository
    {
        public string TableName { get; set; }
        public string ResourceId { get; set; }

        public IEnumerable<DbGetCustomFieldDefinitionResult> Execute()
        {
            return new List<DbGetCustomFieldDefinitionResult>();
        }
    }
}