// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface IKanbanDetailRepository
    {
        /// <summary>
        /// Counts the number of KanbanDetail in IKanbanDetailRepository.
        /// </summary>
        /// <returns>Returns the count of IKanbanDetailRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of KanbanDetail. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of KanbanDetail.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> GetAll();

        /// <summary>
        /// Returns all instances of KanbanDetail to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of KanbanDetail.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the KanbanDetail against kanbanDetailId. 
        /// </summary>
        /// <param name="kanbanDetailId">The column "kanban_detail_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of KanbanDetail.</returns>
        Frapid.Config.Entities.KanbanDetail Get(long kanbanDetailId);

        /// <summary>
        /// Gets the first record of KanbanDetail.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of KanbanDetail.</returns>
        Frapid.Config.Entities.KanbanDetail GetFirst();

        /// <summary>
        /// Gets the previous record of KanbanDetail sorted by kanbanDetailId. 
        /// </summary>
        /// <param name="kanbanDetailId">The column "kanban_detail_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of KanbanDetail.</returns>
        Frapid.Config.Entities.KanbanDetail GetPrevious(long kanbanDetailId);

        /// <summary>
        /// Gets the next record of KanbanDetail sorted by kanbanDetailId. 
        /// </summary>
        /// <param name="kanbanDetailId">The column "kanban_detail_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of KanbanDetail.</returns>
        Frapid.Config.Entities.KanbanDetail GetNext(long kanbanDetailId);

        /// <summary>
        /// Gets the last record of KanbanDetail.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of KanbanDetail.</returns>
        Frapid.Config.Entities.KanbanDetail GetLast();

        /// <summary>
        /// Returns multiple instances of the KanbanDetail against kanbanDetailIds. 
        /// </summary>
        /// <param name="kanbanDetailIds">Array of column "kanban_detail_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of KanbanDetail.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> Get(long[] kanbanDetailIds);

        /// <summary>
        /// Custom fields are user defined form elements for IKanbanDetailRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for KanbanDetail.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding KanbanDetail.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for KanbanDetail.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of KanbanDetail class to IKanbanDetailRepository.
        /// </summary>
        /// <param name="kanbanDetail">The instance of KanbanDetail class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic kanbanDetail, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of KanbanDetail class to IKanbanDetailRepository.
        /// </summary>
        /// <param name="kanbanDetail">The instance of KanbanDetail class to insert.</param>
        object Add(dynamic kanbanDetail);

        /// <summary>
        /// Inserts or updates multiple instances of KanbanDetail class to IKanbanDetailRepository.;
        /// </summary>
        /// <param name="kanbanDetails">List of KanbanDetail class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> kanbanDetails);


        /// <summary>
        /// Updates IKanbanDetailRepository with an instance of KanbanDetail class against the primary key value.
        /// </summary>
        /// <param name="kanbanDetail">The instance of KanbanDetail class to update.</param>
        /// <param name="kanbanDetailId">The value of the column "kanban_detail_id" which will be updated.</param>
        void Update(dynamic kanbanDetail, long kanbanDetailId);

        /// <summary>
        /// Deletes KanbanDetail from  IKanbanDetailRepository against the primary key value.
        /// </summary>
        /// <param name="kanbanDetailId">The value of the column "kanban_detail_id" which will be deleted.</param>
        void Delete(long kanbanDetailId);

        /// <summary>
        /// Produces a paginated result of 10 KanbanDetail classes.
        /// </summary>
        /// <returns>Returns the first page of collection of KanbanDetail class.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 KanbanDetail classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of KanbanDetail class.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IKanbanDetailRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of KanbanDetail class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IKanbanDetailRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of KanbanDetail class.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IKanbanDetailRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of KanbanDetail class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IKanbanDetailRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of KanbanDetail class.</returns>
        IEnumerable<Frapid.Config.Entities.KanbanDetail> GetFiltered(long pageNumber, string filterName);



        IEnumerable<Frapid.Config.Entities.KanbanDetail> Get(long[] kanbanIds, object[] resourceIds);
    }
}