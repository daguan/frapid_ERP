// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface ICompleteResetRepository
    {

        System.Guid RequestId { get; set; }
        string Password { get; set; }

        /// <summary>
        /// Prepares and executes ICompleteResetRepository.
        /// </summary>
        void Execute();
    }
}