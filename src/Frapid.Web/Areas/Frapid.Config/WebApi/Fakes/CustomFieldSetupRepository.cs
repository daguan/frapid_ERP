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
    public class CustomFieldSetupRepository : ICustomFieldSetupRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public Frapid.Config.Entities.CustomFieldSetup Get(int customFieldSetupId)
        {
            return new Frapid.Config.Entities.CustomFieldSetup();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> Get(int[] customFieldSetupIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public Frapid.Config.Entities.CustomFieldSetup GetFirst()
        {
            return new Frapid.Config.Entities.CustomFieldSetup();
        }

        public Frapid.Config.Entities.CustomFieldSetup GetPrevious(int customFieldSetupId)
        {
            return new Frapid.Config.Entities.CustomFieldSetup();
        }

        public Frapid.Config.Entities.CustomFieldSetup GetNext(int customFieldSetupId)
        {
            return new Frapid.Config.Entities.CustomFieldSetup();
        }

        public Frapid.Config.Entities.CustomFieldSetup GetLast()
        {
            return new Frapid.Config.Entities.CustomFieldSetup();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.CustomFieldSetup> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.CustomFieldSetup(), 1);
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

        public object AddOrEdit(dynamic customFieldSetup, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic customFieldSetup, int customFieldSetupId)
        {
            if (customFieldSetupId > 0)
            {
                return;
            }

            throw new ArgumentException("customFieldSetupId is null.");
        }

        public object Add(dynamic customFieldSetup)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> customFieldSetups)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int customFieldSetupId)
        {
            if (customFieldSetupId > 0)
            {
                return;
            }

            throw new ArgumentException("customFieldSetupId is null.");
        }


    }
}