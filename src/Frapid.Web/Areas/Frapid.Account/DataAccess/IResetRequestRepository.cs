// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IResetRequestRepository
    {
        /// <summary>
        /// Counts the number of ResetRequest in IResetRequestRepository.
        /// </summary>
        /// <returns>Returns the count of IResetRequestRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of ResetRequest. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of ResetRequest.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> GetAll();

        /// <summary>
        /// Returns all instances of ResetRequest to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of ResetRequest.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the ResetRequest against requestId. 
        /// </summary>
        /// <param name="requestId">The column "request_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of ResetRequest.</returns>
        Frapid.Account.Entities.ResetRequest Get(System.Guid requestId);

        /// <summary>
        /// Gets the first record of ResetRequest.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of ResetRequest.</returns>
        Frapid.Account.Entities.ResetRequest GetFirst();

        /// <summary>
        /// Gets the previous record of ResetRequest sorted by requestId. 
        /// </summary>
        /// <param name="requestId">The column "request_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of ResetRequest.</returns>
        Frapid.Account.Entities.ResetRequest GetPrevious(System.Guid requestId);

        /// <summary>
        /// Gets the next record of ResetRequest sorted by requestId. 
        /// </summary>
        /// <param name="requestId">The column "request_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of ResetRequest.</returns>
        Frapid.Account.Entities.ResetRequest GetNext(System.Guid requestId);

        /// <summary>
        /// Gets the last record of ResetRequest.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of ResetRequest.</returns>
        Frapid.Account.Entities.ResetRequest GetLast();

        /// <summary>
        /// Returns multiple instances of the ResetRequest against requestIds. 
        /// </summary>
        /// <param name="requestIds">Array of column "request_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of ResetRequest.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> Get(System.Guid[] requestIds);

        /// <summary>
        /// Custom fields are user defined form elements for IResetRequestRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for ResetRequest.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding ResetRequest.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for ResetRequest.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of ResetRequest class to IResetRequestRepository.
        /// </summary>
        /// <param name="resetRequest">The instance of ResetRequest class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic resetRequest, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of ResetRequest class to IResetRequestRepository.
        /// </summary>
        /// <param name="resetRequest">The instance of ResetRequest class to insert.</param>
        object Add(dynamic resetRequest);

        /// <summary>
        /// Inserts or updates multiple instances of ResetRequest class to IResetRequestRepository.;
        /// </summary>
        /// <param name="resetRequests">List of ResetRequest class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> resetRequests);


        /// <summary>
        /// Updates IResetRequestRepository with an instance of ResetRequest class against the primary key value.
        /// </summary>
        /// <param name="resetRequest">The instance of ResetRequest class to update.</param>
        /// <param name="requestId">The value of the column "request_id" which will be updated.</param>
        void Update(dynamic resetRequest, System.Guid requestId);

        /// <summary>
        /// Deletes ResetRequest from  IResetRequestRepository against the primary key value.
        /// </summary>
        /// <param name="requestId">The value of the column "request_id" which will be deleted.</param>
        void Delete(System.Guid requestId);

        /// <summary>
        /// Produces a paginated result of 10 ResetRequest classes.
        /// </summary>
        /// <returns>Returns the first page of collection of ResetRequest class.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 ResetRequest classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of ResetRequest class.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IResetRequestRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of ResetRequest class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IResetRequestRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of ResetRequest class.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IResetRequestRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of ResetRequest class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IResetRequestRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of ResetRequest class.</returns>
        IEnumerable<Frapid.Account.Entities.ResetRequest> GetFiltered(long pageNumber, string filterName);



    }
}