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
    public class AppRepository : IAppRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.App> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public Frapid.Core.Entities.App Get(string appName)
        {
            return new Frapid.Core.Entities.App();
        }

        public IEnumerable<Frapid.Core.Entities.App> Get(string[] appNames)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public Frapid.Core.Entities.App GetFirst()
        {
            return new Frapid.Core.Entities.App();
        }

        public Frapid.Core.Entities.App GetPrevious(string appName)
        {
            return new Frapid.Core.Entities.App();
        }

        public Frapid.Core.Entities.App GetNext(string appName)
        {
            return new Frapid.Core.Entities.App();
        }

        public Frapid.Core.Entities.App GetLast()
        {
            return new Frapid.Core.Entities.App();
        }

        public IEnumerable<Frapid.Core.Entities.App> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public IEnumerable<Frapid.Core.Entities.App> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.App> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Core.Entities.App> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.App(), 1);
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

        public object AddOrEdit(dynamic app, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic app, string appName)
        {
            if (!string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            throw new ArgumentException("appName is null.");
        }

        public object Add(dynamic app)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> apps)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(string appName)
        {
            if (!string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            throw new ArgumentException("appName is null.");
        }


    }
}