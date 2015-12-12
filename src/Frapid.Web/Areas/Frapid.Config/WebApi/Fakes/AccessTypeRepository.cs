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
    public class AccessTypeRepository : IAccessTypeRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public Frapid.Config.Entities.AccessType Get(int accessTypeId)
        {
            return new Frapid.Config.Entities.AccessType();
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> Get(int[] accessTypeIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public Frapid.Config.Entities.AccessType GetFirst()
        {
            return new Frapid.Config.Entities.AccessType();
        }

        public Frapid.Config.Entities.AccessType GetPrevious(int accessTypeId)
        {
            return new Frapid.Config.Entities.AccessType();
        }

        public Frapid.Config.Entities.AccessType GetNext(int accessTypeId)
        {
            return new Frapid.Config.Entities.AccessType();
        }

        public Frapid.Config.Entities.AccessType GetLast()
        {
            return new Frapid.Config.Entities.AccessType();
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.AccessType> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AccessType(), 1);
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

        public object AddOrEdit(dynamic accessType, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic accessType, int accessTypeId)
        {
            if (accessTypeId > 0)
            {
                return;
            }

            throw new ArgumentException("accessTypeId is null.");
        }

        public object Add(dynamic accessType)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> accessTypes)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int accessTypeId)
        {
            if (accessTypeId > 0)
            {
                return;
            }

            throw new ArgumentException("accessTypeId is null.");
        }


    }
}