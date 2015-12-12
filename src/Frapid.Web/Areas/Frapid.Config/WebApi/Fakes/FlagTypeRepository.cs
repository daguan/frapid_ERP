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
    public class FlagTypeRepository : IFlagTypeRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public Frapid.Config.Entities.FlagType Get(int flagTypeId)
        {
            return new Frapid.Config.Entities.FlagType();
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> Get(int[] flagTypeIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public Frapid.Config.Entities.FlagType GetFirst()
        {
            return new Frapid.Config.Entities.FlagType();
        }

        public Frapid.Config.Entities.FlagType GetPrevious(int flagTypeId)
        {
            return new Frapid.Config.Entities.FlagType();
        }

        public Frapid.Config.Entities.FlagType GetNext(int flagTypeId)
        {
            return new Frapid.Config.Entities.FlagType();
        }

        public Frapid.Config.Entities.FlagType GetLast()
        {
            return new Frapid.Config.Entities.FlagType();
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.FlagType> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.FlagType(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields()
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId)
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public object AddOrEdit(dynamic flagType, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic flagType, int flagTypeId)
        {
            if (flagTypeId > 0)
            {
                return;
            }

            throw new ArgumentException("flagTypeId is null.");
        }

        public object Add(dynamic flagType)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> flagTypes)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int flagTypeId)
        {
            if (flagTypeId > 0)
            {
                return;
            }

            throw new ArgumentException("flagTypeId is null.");
        }


    }
}