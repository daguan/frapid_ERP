// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IResetAccountRepository
    {

        string Email { get; set; }
        string Browser { get; set; }
        string IpAddress { get; set; }

        /// <summary>
        /// Prepares and executes IResetAccountRepository.
        /// </summary>
        IEnumerable<DbResetAccountResult> Execute();
    }
}