// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface ICreateFlagRepository
    {

        int UserId { get; set; }
        int FlagTypeId { get; set; }
        string Resource { get; set; }
        string ResourceKey { get; set; }
        string ResourceId { get; set; }

        /// <summary>
        /// Prepares and executes ICreateFlagRepository.
        /// </summary>
        void Execute();
    }
}