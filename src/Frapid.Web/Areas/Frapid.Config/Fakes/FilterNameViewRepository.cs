// ReSharper disable All

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Config.DataAccess;
using Filter = Frapid.DataAccess.Filter;
using FilterNameView = Frapid.Config.Entities.FilterNameView;

namespace Frapid.Config.Api.Fakes
{
    public class FilterNameViewRepository : IFilterNameViewRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<FilterNameView> Get()
        {
            return Enumerable.Repeat(new FilterNameView(), 1);
        }

        public IEnumerable<FilterNameView> GetPaginatedResult()
        {
            return Enumerable.Repeat(new FilterNameView(), 1);
        }

        public IEnumerable<FilterNameView> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new FilterNameView(), 1);
        }

        public List<Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Filter(), 1).ToList();
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public IEnumerable<FilterNameView> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new FilterNameView(), 1);
        }

        public long CountWhere(List<Filter> filters)
        {
            return 1;
        }

        public IEnumerable<FilterNameView> GetWhere(long pageNumber, List<Filter> filters)
        {
            return Enumerable.Repeat(new FilterNameView(), 1);
        }
    }
}