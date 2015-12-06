// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IGetCustomFieldDefinitionRepository
    {

        string TableName { get; set; }
        string ResourceId { get; set; }

        /// <summary>
        /// Prepares and executes IGetCustomFieldDefinitionRepository.
        /// </summary>
        IEnumerable<DbGetCustomFieldDefinitionResult> Execute();
    }
}