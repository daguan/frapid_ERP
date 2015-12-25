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
    public class MenuLocaleRepository : IMenuLocaleRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public Frapid.Core.Entities.MenuLocale Get(int menuLocaleId)
        {
            return new Frapid.Core.Entities.MenuLocale();
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> Get(int[] menuLocaleIds)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public Frapid.Core.Entities.MenuLocale GetFirst()
        {
            return new Frapid.Core.Entities.MenuLocale();
        }

        public Frapid.Core.Entities.MenuLocale GetPrevious(int menuLocaleId)
        {
            return new Frapid.Core.Entities.MenuLocale();
        }

        public Frapid.Core.Entities.MenuLocale GetNext(int menuLocaleId)
        {
            return new Frapid.Core.Entities.MenuLocale();
        }

        public Frapid.Core.Entities.MenuLocale GetLast()
        {
            return new Frapid.Core.Entities.MenuLocale();
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Core.Entities.MenuLocale> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Core.Entities.MenuLocale(), 1);
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

        public object AddOrEdit(dynamic menuLocale, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic menuLocale, int menuLocaleId)
        {
            if (menuLocaleId > 0)
            {
                return;
            }

            throw new ArgumentException("menuLocaleId is null.");
        }

        public object Add(dynamic menuLocale)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> menuLocales)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(int menuLocaleId)
        {
            if (menuLocaleId > 0)
            {
                return;
            }

            throw new ArgumentException("menuLocaleId is null.");
        }


    }
}