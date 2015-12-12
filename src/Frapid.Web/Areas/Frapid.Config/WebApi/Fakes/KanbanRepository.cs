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

        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            return 1;
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
        }

        public long CountFiltered(string filterName)
        {
            return 1;
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            return Enumerable.Repeat(new Frapid.DataAccess.Models.Filter(), 1).ToList();
        }

        public IEnumerable<Frapid.Config.Entities.Kanban> GetFiltered(long pageNumber, string filterName)
        {
            return Enumerable.Repeat(new Frapid.Config.Entities.Kanban(), 1);
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

        public object AddOrEdit(dynamic kanban, List<Frapid.DataAccess.Models.CustomField> customFields)
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