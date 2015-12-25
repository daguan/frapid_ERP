// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IApplicationRepository
    {
        /// <summary>
        /// Counts the number of Application in IApplicationRepository.
        /// </summary>
        /// <returns>Returns the count of IApplicationRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Application. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Application.</returns>
        IEnumerable<Frapid.Account.Entities.Application> GetAll();

        /// <summary>
        /// Returns all instances of Application to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Application.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Application against applicationId. 
        /// </summary>
        /// <param name="applicationId">The column "application_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Application.</returns>
        Frapid.Account.Entities.Application Get(System.Guid applicationId);

        /// <summary>
        /// Gets the first record of Application.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Application.</returns>
        Frapid.Account.Entities.Application GetFirst();

        /// <summary>
        /// Gets the previous record of Application sorted by applicationId. 
        /// </summary>
        /// <param name="applicationId">The column "application_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Application.</returns>
        Frapid.Account.Entities.Application GetPrevious(System.Guid applicationId);

        /// <summary>
        /// Gets the next record of Application sorted by applicationId. 
        /// </summary>
        /// <param name="applicationId">The column "application_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Application.</returns>
        Frapid.Account.Entities.Application GetNext(System.Guid applicationId);

        /// <summary>
        /// Gets the last record of Application.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Application.</returns>
        Frapid.Account.Entities.Application GetLast();

        /// <summary>
        /// Returns multiple instances of the Application against applicationIds. 
        /// </summary>
        /// <param name="applicationIds">Array of column "application_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Application.</returns>
        IEnumerable<Frapid.Account.Entities.Application> Get(System.Guid[] applicationIds);

        /// <summary>
        /// Custom fields are user defined form elements for IApplicationRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Application.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Application.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Application.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Application class to IApplicationRepository.
        /// </summary>
        /// <param name="application">The instance of Application class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic application, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Application class to IApplicationRepository.
        /// </summary>
        /// <param name="application">The instance of Application class to insert.</param>
        object Add(dynamic application);

        /// <summary>
        /// Inserts or updates multiple instances of Application class to IApplicationRepository.;
        /// </summary>
        /// <param name="applications">List of Application class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> applications);


        /// <summary>
        /// Updates IApplicationRepository with an instance of Application class against the primary key value.
        /// </summary>
        /// <param name="application">The instance of Application class to update.</param>
        /// <param name="applicationId">The value of the column "application_id" which will be updated.</param>
        void Update(dynamic application, System.Guid applicationId);

        /// <summary>
        /// Deletes Application from  IApplicationRepository against the primary key value.
        /// </summary>
        /// <param name="applicationId">The value of the column "application_id" which will be deleted.</param>
        void Delete(System.Guid applicationId);

        /// <summary>
        /// Produces a paginated result of 10 Application classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Application class.</returns>
        IEnumerable<Frapid.Account.Entities.Application> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Application classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Application class.</returns>
        IEnumerable<Frapid.Account.Entities.Application> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IApplicationRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Application class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IApplicationRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Application class.</returns>
        IEnumerable<Frapid.Account.Entities.Application> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IApplicationRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Application class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IApplicationRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Application class.</returns>
        IEnumerable<Frapid.Account.Entities.Application> GetFiltered(long pageNumber, string filterName);



    }
}