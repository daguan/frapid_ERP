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
    public class DefaultEntityAccessRepository : IDefaultEntityAccessRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public Frapid.Config.Entities.DefaultEntityAccess Get(int defaultEntityAccessId)
        {
            return new Frapid.Config.Entities.DefaultEntityAccess();
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> Get(int[] defaultEntityAccessIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public Frapid.Config.Entities.DefaultEntityAccess GetFirst()
        {
            return new Frapid.Config.Entities.DefaultEntityAccess();
        }

        public Frapid.Config.Entities.DefaultEntityAccess GetPrevious(int defaultEntityAccessId)
        {
            return new Frapid.Config.Entities.DefaultEntityAccess();
        }

        public Frapid.Config.Entities.DefaultEntityAccess GetNext(int defaultEntityAccessId)
        {
            return new Frapid.Config.Entities.DefaultEntityAccess();
        }

        public Frapid.Config.Entities.DefaultEntityAccess GetLast()
        {
            return new Frapid.Config.Entities.DefaultEntityAccess();
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.DefaultEntityAccess(), 1);
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

        public object AddOrEdit(dynamic defaultEntityAccess, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic defaultEntityAccess, int defaultEntityAccessId)
        {
            if (defaultEntityAccessId > 0)
            {
                return;
            }

            throw new ArgumentException("defaultEntityAccessId is null.");
        }

        public object Add(dynamic defaultEntityAccess)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> defaultEntityAccesses)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int defaultEntityAccessId)
        {
            if (defaultEntityAccessId > 0)
            {
                return;
            }

            throw new ArgumentException("defaultEntityAccessId is null.");
        }


    }
}