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
    public class CustomFieldDataTypeRepository : ICustomFieldDataTypeRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public Frapid.Config.Entities.CustomFieldDataType Get(string dataType)
        {
            return new Frapid.Config.Entities.CustomFieldDataType();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> Get(string[] dataTypes)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public Frapid.Config.Entities.CustomFieldDataType GetFirst()
        {
            return new Frapid.Config.Entities.CustomFieldDataType();
        }

        public Frapid.Config.Entities.CustomFieldDataType GetPrevious(string dataType)
        {
            return new Frapid.Config.Entities.CustomFieldDataType();
        }

        public Frapid.Config.Entities.CustomFieldDataType GetNext(string dataType)
        {
            return new Frapid.Config.Entities.CustomFieldDataType();
        }

        public Frapid.Config.Entities.CustomFieldDataType GetLast()
        {
            return new Frapid.Config.Entities.CustomFieldDataType();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldDataType> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldDataType(), 1);
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

        public object AddOrEdit(dynamic customFieldDataType, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic customFieldDataType, string dataType)
        {
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                return;
            }

            throw new ArgumentException("dataType is null.");
        }

        public object Add(dynamic customFieldDataType)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> customFieldDataTypes)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(string dataType)
        {
            if (!string.IsNullOrWhiteSpace(dataType))
            {
                return;
            }

            throw new ArgumentException("dataType is null.");
        }


    }
}