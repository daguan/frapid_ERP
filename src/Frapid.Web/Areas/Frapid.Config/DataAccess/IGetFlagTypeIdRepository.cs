// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IGetFlagTypeIdRepository
    {

        int UserId { get; set; }
        string Resource { get; set; }
        string ResourceKey { get; set; }
        string ResourceId { get; set; }

        /// <summary>
        /// Prepares and executes IGetFlagTypeIdRepository.
        /// </summary>
        int Execute();
    }
}