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
    public class AppDependencyRepository : IAppDependencyRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public Frapid.Config.Entities.AppDependency Get(int appDependencyId)
        {
            return new Frapid.Config.Entities.AppDependency();
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> Get(int[] appDependencyIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public Frapid.Config.Entities.AppDependency GetFirst()
        {
            return new Frapid.Config.Entities.AppDependency();
        }

        public Frapid.Config.Entities.AppDependency GetPrevious(int appDependencyId)
        {
            return new Frapid.Config.Entities.AppDependency();
        }

        public Frapid.Config.Entities.AppDependency GetNext(int appDependencyId)
        {
            return new Frapid.Config.Entities.AppDependency();
        }

        public Frapid.Config.Entities.AppDependency GetLast()
        {
            return new Frapid.Config.Entities.AppDependency();
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.AppDependency> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.AppDependency(), 1);
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

        public object AddOrEdit(dynamic appDependency, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic appDependency, int appDependencyId)
        {
            if (appDependencyId > 0)
            {
                return;
            }

            throw new ArgumentException("appDependencyId is null.");
        }

        public object Add(dynamic appDependency)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> appDependencies)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int appDependencyId)
        {
            if (appDependencyId > 0)
            {
                return;
            }

            throw new ArgumentException("appDependencyId is null.");
        }


    }
}