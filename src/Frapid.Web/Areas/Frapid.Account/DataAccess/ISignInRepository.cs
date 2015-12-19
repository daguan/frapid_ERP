// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface ISignInRepository
    {

        string Email { get; set; }
        int OfficeId { get; set; }
        string Challenge { get; set; }
        string Password { get; set; }
        string Browser { get; set; }
        string IpAddress { get; set; }
        string Culture { get; set; }

        /// <summary>
        /// Prepares and executes ISignInRepository.
        /// </summary>
        IEnumerable<DbSignInResult> Execute();
    }
}