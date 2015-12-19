// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Account.Entities;
namespace Frapid.Account.DataAccess
{
    public interface IFbSignInRepository
    {

        string FbUserId { get; set; }
        string Email { get; set; }
        int OfficeId { get; set; }
        string Name { get; set; }
        string Token { get; set; }
        string Browser { get; set; }
        string IpAddress { get; set; }
        string Culture { get; set; }

        /// <summary>
        /// Prepares and executes IFbSignInRepository.
        /// </summary>
        IEnumerable<DbFbSignInResult> Execute();
    }
}