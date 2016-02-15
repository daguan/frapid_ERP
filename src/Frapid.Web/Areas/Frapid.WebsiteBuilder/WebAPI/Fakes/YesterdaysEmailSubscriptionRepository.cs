// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.WebsiteBuilder.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.WebsiteBuilder.Api.Fakes
{
    public class YesterdaysEmailSubscriptionRepository : IYesterdaysEmailSubscriptionRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> Get()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription(), 1);
        }



        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription(), 1);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.YesterdaysEmailSubscription(), 1);
        }

    }
}