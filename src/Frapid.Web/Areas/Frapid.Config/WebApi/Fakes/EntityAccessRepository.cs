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
    public class EntityAccessRepository : IEntityAccessRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public Frapid.Config.Entities.EntityAccess Get(int entityAccessId)
        {
            return new Frapid.Config.Entities.EntityAccess();
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> Get(int[] entityAccessIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public Frapid.Config.Entities.EntityAccess GetFirst()
        {
            return new Frapid.Config.Entities.EntityAccess();
        }

        public Frapid.Config.Entities.EntityAccess GetPrevious(int entityAccessId)
        {
            return new Frapid.Config.Entities.EntityAccess();
        }

        public Frapid.Config.Entities.EntityAccess GetNext(int entityAccessId)
        {
            return new Frapid.Config.Entities.EntityAccess();
        }

        public Frapid.Config.Entities.EntityAccess GetLast()
        {
            return new Frapid.Config.Entities.EntityAccess();
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.EntityAccess> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.EntityAccess(), 1);
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

        public object AddOrEdit(dynamic entityAccess, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic entityAccess, int entityAccessId)
        {
            if (entityAccessId > 0)
            {
                return;
            }

            throw new ArgumentException("entityAccessId is null.");
        }

        public object Add(dynamic entityAccess)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> entityAccesses)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int entityAccessId)
        {
            if (entityAccessId > 0)
            {
                return;
            }

            throw new ArgumentException("entityAccessId is null.");
        }


    }
}