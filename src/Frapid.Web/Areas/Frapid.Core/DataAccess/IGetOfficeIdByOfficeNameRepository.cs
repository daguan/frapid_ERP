// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Core.Entities;
namespace Frapid.Core.DataAccess
{
    public interface IGetOfficeIdByOfficeNameRepository
    {

        string OfficeName { get; set; }

        /// <summary>
        /// Prepares and executes IGetOfficeIdByOfficeNameRepository.
        /// </summary>
        int Execute();
    }
}