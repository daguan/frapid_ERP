// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.WebsiteBuilder.Entities;

namespace Frapid.WebsiteBuilder.Api.Fakes
{
    public class GetCategoryIdByCategoryAliasRepository : IGetCategoryIdByCategoryAliasRepository
    {
        public string Alias { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}