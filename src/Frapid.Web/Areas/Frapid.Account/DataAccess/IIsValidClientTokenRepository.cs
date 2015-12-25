// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IIsValidClientTokenRepository
    {

        string ClientToken { get; set; }
        string IpAddress { get; set; }
        string UserAgent { get; set; }

        /// <summary>
        /// Prepares and executes IIsValidClientTokenRepository.
        /// </summary>
        bool Execute();
    }
}