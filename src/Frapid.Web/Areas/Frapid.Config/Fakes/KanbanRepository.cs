// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Http;
using Frapid.Config.DataAccess;
using Frapid.DataAccess;
using Filter = Frapid.DataAccess.Filter;

namespace Frapid.Config.Api.Fakes
{
    public class KanbanRepository : IKanbanRepository
    {
        public long Count()
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetAll()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public IEnumerable<dynamic> Export()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public Frapid.Config.Entities.Kanban Get(long kanbanId)
        {
            return new Frapid.Config.Entities.Kanban();
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> Get(long[] kanbanIds)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public Frapid.Config.Entities.Kanban GetFirst()
        {
            return new Frapid.Config.Entities.Kanban();
        }

        public Frapid.Config.Entities.Kanban GetPrevious(long kanbanId)
        {
            return new Frapid.Config.Entities.Kanban();
        }

        public Frapid.Config.Entities.Kanban GetNext(long kanbanId)
        {
            return new Frapid.Config.Entities.Kanban();
        }

        public Frapid.Config.Entities.Kanban GetLast()
        {
            return new Frapid.Config.Entities.Kanban();
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetPaginatedResult()
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetPaginatedResult(long pageNumber)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public long CountWhere(List<Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetWhere(long pageNumber, List<Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            return Enumerable.Repeat(new DisplayField(), 1);
        }

        public IEnumerable<CustomField> GetCustomFields()
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public IEnumerable<CustomField> GetCustomFields(string resourceId)
        {
            return Enumerable.Repeat(new CustomField(), 1);
        }

        public object AddOrEdit(dynamic kanban, List<CustomField> customFields)
        {
            return true;
        }

        public void Update(dynamic kanban, long kanbanId)
        {
            if (kanbanId > 0)
            {
                return;
            }

            throw new ArgumentException("kanbanId is null.");
        }

        public object Add(dynamic kanban)
        {
            return true;
        }

        public List<object> BulkImport(List<ExpandoObject> kanbans)
        {
            return Enumerable.Repeat(new object(), 1).ToList();
        }

        public void Delete(long kanbanId)
        {
            if (kanbanId > 0)
            {
                return;
            }

            throw new ArgumentException("kanbanId is null.");
        }


    }
}