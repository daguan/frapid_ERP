// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.WebsiteBuilder.Entities;
namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IGetCategoryIdByCategoryAliasRepository
    {

        string Alias { get; set; }

        /// <summary>
        /// Prepares and executes IGetCategoryIdByCategoryAliasRepository.
        /// </summary>
        int Execute();
    }
}