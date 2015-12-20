// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.WebsiteBuilder.Entities;
namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IRemoveEmailSubscriptionRepository
    {

        string Email { get; set; }

        /// <summary>
        /// Prepares and executes IRemoveEmailSubscriptionRepository.
        /// </summary>
        bool Execute();
    }
}