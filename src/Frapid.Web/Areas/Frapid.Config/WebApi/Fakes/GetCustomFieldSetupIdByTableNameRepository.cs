// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class GetCustomFieldSetupIdByTableNameRepository : IGetCustomFieldSetupIdByTableNameRepository
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}