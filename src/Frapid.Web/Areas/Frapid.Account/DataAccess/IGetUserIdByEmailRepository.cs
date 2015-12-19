// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IGetUserIdByEmailRepository
    {

        string Email { get; set; }

        /// <summary>
        /// Prepares and executes IGetUserIdByEmailRepository.
        /// </summary>
        int Execute();
    }
}