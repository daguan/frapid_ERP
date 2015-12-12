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
    public class AppRepository : IAppRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.App> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public Frapid.Config.Entities.App Get(string appName)
        {
            return new Frapid.Config.Entities.App();
        }

        public IEnumerable<Frapid.Config.Entities.App> Get(string[] appNames)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public Frapid.Config.Entities.App GetFirst()
        {
            return new Frapid.Config.Entities.App();
        }

        public Frapid.Config.Entities.App GetPrevious(string appName)
        {
            return new Frapid.Config.Entities.App();
        }

        public Frapid.Config.Entities.App GetNext(string appName)
        {
            return new Frapid.Config.Entities.App();
        }

        public Frapid.Config.Entities.App GetLast()
        {
            return new Frapid.Config.Entities.App();
        }

        public IEnumerable<Frapid.Config.Entities.App> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.App> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.App> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.App> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.App(), 1);
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