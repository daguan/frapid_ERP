// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.Core.DataAccess;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using CustomField = Frapid.DataAccess.Models.CustomField;

namespace Frapid.Core.Api.Fakes
{
    public class OfficeRepository : IOfficeRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.Office> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public Frapid.Core.Entities.Office Get(int officeId)
        {
            return new Frapid.Core.Entities.Office();
        }

        public IEnumerable<Frapid.Core.Entities.Office> Get(int[] officeIds)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public Frapid.Core.Entities.Office GetFirst()
        {
            return new Frapid.Core.Entities.Office();
        }

        public Frapid.Core.Entities.Office GetPrevious(int officeId)
        {
            return new Frapid.Core.Entities.Office();
        }

        public Frapid.Core.Entities.Office GetNext(int officeId)
        {
            return new Frapid.Core.Entities.Office();
        }

        public Frapid.Core.Entities.Office GetLast()
        {
            return new Frapid.Core.Entities.Office();
        }

        public IEnumerable<Frapid.Core.Entities.Office> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public IEnumerable<Frapid.Core.Entities.Office> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.Office> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Core.Entities.Office> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.Office(), 1);
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

        public object AddOrEdit(dynamic office, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic office, int officeId)
        {
            if (officeId > 0)
            {
                return;
            }

            throw new ArgumentException("officeId is null.");
        }

        public object Add(dynamic office)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> offices)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int officeId)
        {
            if (officeId > 0)
            {
                return;
            }

            throw new ArgumentException("officeId is null.");
        }


    }
}