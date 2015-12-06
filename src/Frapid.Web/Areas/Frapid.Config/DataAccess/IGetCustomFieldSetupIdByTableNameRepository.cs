// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IGetCustomFieldSetupIdByTableNameRepository
    {

        string SchemaName { get; set; }
        string TableName { get; set; }
        string FieldName { get; set; }

        /// <summary>
        /// Prepares and executes IGetCustomFieldSetupIdByTableNameRepository.
        /// </summary>
        int Execute();
    }
}