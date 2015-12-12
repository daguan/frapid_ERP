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
    public class FlagRepository : IFlagRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Flag> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public Frapid.Config.Entities.Flag Get(long flagId)
        {
            return new Frapid.Config.Entities.Flag();
        }

        public IEnumerable<Frapid.Config.Entities.Flag> Get(long[] flagIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public Frapid.Config.Entities.Flag GetFirst()
        {
            return new Frapid.Config.Entities.Flag();
        }

        public Frapid.Config.Entities.Flag GetPrevious(long flagId)
        {
            return new Frapid.Config.Entities.Flag();
        }

        public Frapid.Config.Entities.Flag GetNext(long flagId)
        {
            return new Frapid.Config.Entities.Flag();
        }

        public Frapid.Config.Entities.Flag GetLast()
        {
            return new Frapid.Config.Entities.Flag();
        }

        public IEnumerable<Frapid.Config.Entities.Flag> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.Flag> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Flag> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.Flag> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Flag(), 1);
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

        public object AddOrEdit(dynamic flag, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic flag, long flagId)
        {
            if (flagId > 0)
            {
                return;
            }

            throw new ArgumentException("flagId is null.");
        }

        public object Add(dynamic flag)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> flags)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long flagId)
        {
            if (flagId > 0)
            {
                return;
            }

            throw new ArgumentException("flagId is null.");
        }


    }
}