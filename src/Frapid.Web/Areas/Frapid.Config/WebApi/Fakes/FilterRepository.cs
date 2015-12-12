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
    public class FilterRepository : IFilterRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Filter> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public Frapid.Config.Entities.Filter Get(long filterId)
        {
            return new Frapid.Config.Entities.Filter();
        }

        public IEnumerable<Frapid.Config.Entities.Filter> Get(long[] filterIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public Frapid.Config.Entities.Filter GetFirst()
        {
            return new Frapid.Config.Entities.Filter();
        }

        public Frapid.Config.Entities.Filter GetPrevious(long filterId)
        {
            return new Frapid.Config.Entities.Filter();
        }

        public Frapid.Config.Entities.Filter GetNext(long filterId)
        {
            return new Frapid.Config.Entities.Filter();
        }

        public Frapid.Config.Entities.Filter GetLast()
        {
            return new Frapid.Config.Entities.Filter();
        }

        public IEnumerable<Frapid.Config.Entities.Filter> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.Filter> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Filter> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.Filter> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Filter(), 1);
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

        public object AddOrEdit(dynamic filter, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic filter, long filterId)
        {
            if (filterId > 0)
            {
                return;
            }

            throw new ArgumentException("filterId is null.");
        }

        public object Add(dynamic filter)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> filters)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long filterId)
        {
            if (filterId > 0)
            {
                return;
            }

            throw new ArgumentException("filterId is null.");
        }

        public void Delete(string filterName)
        {
            if (string.IsNullOrWhiteSpace(filterName))
            {
                throw new ArgumentException("filterName is null.");
            }
        }

        public void RecreateFilters(string objectName, string filterName, List<Frapid.Config.Entities.Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                throw new ArgumentException("objectName is null.");
            }

            if (string.IsNullOrWhiteSpace(filterName))
            {
                throw new ArgumentException("filterName is null.");
            }
        }

        public void MakeDefault(string objectName, string filterName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                throw new ArgumentException("objectName is null.");
            }

            if (string.IsNullOrWhiteSpace(filterName))
            {
                throw new ArgumentException("filterName is null.");
            }
        }

        public void RemoveDefault(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                throw new ArgumentException("objectName is null.");
            }
        }

    }
}