// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IGetCustomFieldFormNameRepository
    {

        string TableName { get; set; }

        /// <summary>
        /// Prepares and executes IGetCustomFieldFormNameRepository.
        /// </summary>
        string Execute();
    }
}