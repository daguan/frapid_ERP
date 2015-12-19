// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IConfirmRegistrationRepository
    {

        System.Guid Token { get; set; }

        /// <summary>
        /// Prepares and executes IConfirmRegistrationRepository.
        /// </summary>
        bool Execute();
    }
}