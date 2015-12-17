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
    public class PublishedContentViewRepository : IPublishedContentViewRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> Get()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.PublishedContentView(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.PublishedContentView(), 1);
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.PublishedContentView(), 1);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }


        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.PublishedContentView(), 1);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public IEnumerable<Frapid.WebsiteBuilder.Entities.PublishedContentView> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.WebsiteBuilder.Entities.PublishedContentView(), 1);
        }

    }
}