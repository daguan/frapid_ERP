// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.WebsiteBuilder.Entities;
namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IGetCategoryIdByCategoryNameRepository
    {

        string CategoryName { get; set; }

        /// <summary>
        /// Prepares and executes IGetCategoryIdByCategoryNameRepository.
        /// </summary>
        int Execute();
    }
}