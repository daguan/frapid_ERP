// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface IGetUserIdByLoginIdRepository
    {

        long LoginId { get; set; }

        /// <summary>
        /// Prepares and executes IGetUserIdByLoginIdRepository.
        /// </summary>
        int Execute();
    }
}