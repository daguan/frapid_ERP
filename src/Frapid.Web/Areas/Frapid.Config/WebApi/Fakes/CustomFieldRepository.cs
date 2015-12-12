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
    public class CustomFieldRepository : ICustomFieldRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public Frapid.Config.Entities.CustomField Get(long customFieldId)
        {
            return new Frapid.Config.Entities.CustomField();
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> Get(long[] customFieldIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public Frapid.Config.Entities.CustomField GetFirst()
        {
            return new Frapid.Config.Entities.CustomField();
        }

        public Frapid.Config.Entities.CustomField GetPrevious(long customFieldId)
        {
            return new Frapid.Config.Entities.CustomField();
        }

        public Frapid.Config.Entities.CustomField GetNext(long customFieldId)
        {
            return new Frapid.Config.Entities.CustomField();
        }

        public Frapid.Config.Entities.CustomField GetLast()
        {
            return new Frapid.Config.Entities.CustomField();
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.CustomField> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomField(), 1);
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

        public object AddOrEdit(dynamic customField, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic customField, long customFieldId)
        {
            if (customFieldId > 0)
            {
                return;
            }

            throw new ArgumentException("customFieldId is null.");
        }

        public object Add(dynamic customField)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> customFields)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long customFieldId)
        {
            if (customFieldId > 0)
            {
                return;
            }

            throw new ArgumentException("customFieldId is null.");
        }


    }
}