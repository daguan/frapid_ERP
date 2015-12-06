// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IHasAccessRepository
    {

        int UserId { get; set; }
        string Entity { get; set; }
        int AccessTypeId { get; set; }

        /// <summary>
        /// Prepares and executes IHasAccessRepository.
        /// </summary>
        bool Execute();
    }
}