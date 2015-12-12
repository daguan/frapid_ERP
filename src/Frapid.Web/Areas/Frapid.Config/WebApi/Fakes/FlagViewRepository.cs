// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Config.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.Config.Api.Fakes
{
    public class FlagViewRepository : IFlagViewRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.FlagView> Get()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.FlagView> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.FlagView> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }


        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.FlagView> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.FlagView> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }
        public IEnumerable<Frapid.Config.Entities.FlagView> Get(string resource, int userId, object[] resourceIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagView(), 1);
        }

    }
}