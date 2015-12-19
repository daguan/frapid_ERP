// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IFbUserExistsRepository
    {

        int UserId { get; set; }

        /// <summary>
        /// Prepares and executes IFbUserExistsRepository.
        /// </summary>
        bool Execute();
    }
}