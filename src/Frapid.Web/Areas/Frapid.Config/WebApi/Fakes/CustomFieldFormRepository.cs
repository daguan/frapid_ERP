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
    public class CustomFieldFormRepository : ICustomFieldFormRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public Frapid.Config.Entities.CustomFieldForm Get(string formName)
        {
            return new Frapid.Config.Entities.CustomFieldForm();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> Get(string[] formNames)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public Frapid.Config.Entities.CustomFieldForm GetFirst()
        {
            return new Frapid.Config.Entities.CustomFieldForm();
        }

        public Frapid.Config.Entities.CustomFieldForm GetPrevious(string formName)
        {
            return new Frapid.Config.Entities.CustomFieldForm();
        }

        public Frapid.Config.Entities.CustomFieldForm GetNext(string formName)
        {
            return new Frapid.Config.Entities.CustomFieldForm();
        }

        public Frapid.Config.Entities.CustomFieldForm GetLast()
        {
            return new Frapid.Config.Entities.CustomFieldForm();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldForm> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldForm(), 1);
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

        public object AddOrEdit(dynamic customFieldForm, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic customFieldForm, string formName)
        {
            if (!string.IsNullOrWhiteSpace(formName))
            {
                return;
            }

            throw new ArgumentException("formName is null.");
        }

        public object Add(dynamic customFieldForm)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> customFieldForms)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(string formName)
        {
            if (!string.IsNullOrWhiteSpace(formName))
            {
                return;
            }

            throw new ArgumentException("formName is null.");
        }


    }
}