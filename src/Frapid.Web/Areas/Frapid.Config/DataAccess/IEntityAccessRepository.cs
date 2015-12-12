// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface IEntityAccessRepository
    {
        /// <summary>
        /// Counts the number of EntityAccess in IEntityAccessRepository.
        /// </summary>
        /// <returns>Returns the count of IEntityAccessRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of EntityAccess. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of EntityAccess.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> GetAll();

        /// <summary>
        /// Returns all instances of EntityAccess to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of EntityAccess.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the EntityAccess against entityAccessId. 
        /// </summary>
        /// <param name="entityAccessId">The column "entity_access_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of EntityAccess.</returns>
        Frapid.Config.Entities.EntityAccess Get(int entityAccessId);

        /// <summary>
        /// Gets the first record of EntityAccess.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of EntityAccess.</returns>
        Frapid.Config.Entities.EntityAccess GetFirst();

        /// <summary>
        /// Gets the previous record of EntityAccess sorted by entityAccessId. 
        /// </summary>
        /// <param name="entityAccessId">The column "entity_access_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of EntityAccess.</returns>
        Frapid.Config.Entities.EntityAccess GetPrevious(int entityAccessId);

        /// <summary>
        /// Gets the next record of EntityAccess sorted by entityAccessId. 
        /// </summary>
        /// <param name="entityAccessId">The column "entity_access_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of EntityAccess.</returns>
        Frapid.Config.Entities.EntityAccess GetNext(int entityAccessId);

        /// <summary>
        /// Gets the last record of EntityAccess.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of EntityAccess.</returns>
        Frapid.Config.Entities.EntityAccess GetLast();

        /// <summary>
        /// Returns multiple instances of the EntityAccess against entityAccessIds. 
        /// </summary>
        /// <param name="entityAccessIds">Array of column "entity_access_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of EntityAccess.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> Get(int[] entityAccessIds);

        /// <summary>
        /// Custom fields are user defined form elements for IEntityAccessRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for EntityAccess.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding EntityAccess.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for EntityAccess.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of EntityAccess class to IEntityAccessRepository.
        /// </summary>
        /// <param name="entityAccess">The instance of EntityAccess class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic entityAccess, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of EntityAccess class to IEntityAccessRepository.
        /// </summary>
        /// <param name="entityAccess">The instance of EntityAccess class to insert.</param>
        object Add(dynamic entityAccess);

        /// <summary>
        /// Inserts or updates multiple instances of EntityAccess class to IEntityAccessRepository.;
        /// </summary>
        /// <param name="entityAccesses">List of EntityAccess class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> entityAccesses);


        /// <summary>
        /// Updates IEntityAccessRepository with an instance of EntityAccess class against the primary key value.
        /// </summary>
        /// <param name="entityAccess">The instance of EntityAccess class to update.</param>
        /// <param name="entityAccessId">The value of the column "entity_access_id" which will be updated.</param>
        void Update(dynamic entityAccess, int entityAccessId);

        /// <summary>
        /// Deletes EntityAccess from  IEntityAccessRepository against the primary key value.
        /// </summary>
        /// <param name="entityAccessId">The value of the column "entity_access_id" which will be deleted.</param>
        void Delete(int entityAccessId);

        /// <summary>
        /// Produces a paginated result of 10 EntityAccess classes.
        /// </summary>
        /// <returns>Returns the first page of collection of EntityAccess class.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 EntityAccess classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of EntityAccess class.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IEntityAccessRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of EntityAccess class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IEntityAccessRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of EntityAccess class.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IEntityAccessRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of EntityAccess class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IEntityAccessRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of EntityAccess class.</returns>
        IEnumerable<Frapid.Config.Entities.EntityAccess> GetFiltered(long pageNumber, string filterName);



    }
}