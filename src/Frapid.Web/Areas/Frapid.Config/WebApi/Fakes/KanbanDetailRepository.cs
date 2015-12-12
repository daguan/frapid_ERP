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
    public class KanbanDetailRepository : IKanbanDetailRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public Frapid.Config.Entities.KanbanDetail Get(long kanbanDetailId)
        {
            return new Frapid.Config.Entities.KanbanDetail();
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> Get(long[] kanbanDetailIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public Frapid.Config.Entities.KanbanDetail GetFirst()
        {
            return new Frapid.Config.Entities.KanbanDetail();
        }

        public Frapid.Config.Entities.KanbanDetail GetPrevious(long kanbanDetailId)
        {
            return new Frapid.Config.Entities.KanbanDetail();
        }

        public Frapid.Config.Entities.KanbanDetail GetNext(long kanbanDetailId)
        {
            return new Frapid.Config.Entities.KanbanDetail();
        }

        public Frapid.Config.Entities.KanbanDetail GetLast()
        {
            return new Frapid.Config.Entities.KanbanDetail();
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
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

        public object AddOrEdit(dynamic kanbanDetail, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic kanbanDetail, long kanbanDetailId)
        {
            if (kanbanDetailId > 0)
            {
                return;
            }

            throw new ArgumentException("kanbanDetailId is null.");
        }

        public object Add(dynamic kanbanDetail)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> kanbanDetails)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long kanbanDetailId)
        {
            if (kanbanDetailId > 0)
            {
                return;
            }

            throw new ArgumentException("kanbanDetailId is null.");
        }

        public IEnumerable<Frapid.Config.Entities.KanbanDetail> Get(long[] kanbanIds, object[] resourceIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.KanbanDetail(), 1);
        }

    }
}