// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IGetRegistrationRoleIdRepository
    {

        string Email { get; set; }

        /// <summary>
        /// Prepares and executes IGetRegistrationRoleIdRepository.
        /// </summary>
        int Execute();
    }
}