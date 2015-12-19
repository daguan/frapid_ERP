// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IUserExistsRepository
    {

        string Email { get; set; }

        /// <summary>
        /// Prepares and executes IUserExistsRepository.
        /// </summary>
        bool Execute();
    }
}