using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess.Models;

namespace Frapid.WebApi.DataAccess
{
    public interface IFormRepository
    {
        long LoginId { get; set; }
        int OfficeId { get; set; }

        /// <summary>
        ///     Counts the number of rows in IFormRepository.
        /// </summary>
        /// <returns>Returns the count of IFormRepository.</returns>
        long Count();

        /// <summary>
        ///     Returns all instances of IFormRepository.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of IFormRepository.</returns>
        IEnumerable<dynamic> GetAll();

        /// <summary>
        ///     Returns a single instance of the IFormRepository against primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of IFormRepository.</returns>
        dynamic Get(object primaryKey);

        /// <summary>
        ///     Gets the first record of IFormRepository.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of IFormRepository.</returns>
        dynamic GetFirst();

        /// <summary>
        ///     Gets the previous record of IFormRepository sorted by primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key column parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of IFormRepository.</returns>
        dynamic GetPrevious(object primaryKey);

        /// <summary>
        ///     Gets the next record of IFormRepository sorted by primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key column parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of IFormRepository.</returns>
        dynamic GetNext(object primaryKey);

        /// <summary>
        ///     Gets the last record of IFormRepository.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of IFormRepository.</returns>
        dynamic GetLast();

        /// <summary>
        ///     Returns multiple instances of the IFormRepository against primary keys.
        /// </summary>
        /// <param name="primaryKeys">Array of primary key column parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of IFormRepository.</returns>
        IEnumerable<dynamic> Get(object[] primaryKeys);

        /// <summary>
        ///     Custom fields are user defined form elements for IFormRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for IFormRepository.</returns>
        IEnumerable<CustomField> GetCustomFields(string resourceId);

        /// <summary>
        ///     Displayfields provide a minimal name/value context for data binding IFormRepository.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for IFormRepository.</returns>
        IEnumerable<DisplayField> GetDisplayFields();

        /// <summary>
        ///     Inserts the instance of dynamic class to IFormRepository.
        /// </summary>
        /// <param name="form">The dynamic IFormRepository class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic form, List<CustomField> customFields);

        /// <summary>
        ///     Inserts the instance of dynamic class to IFormRepository.
        /// </summary>
        /// <param name="form">The instance of IFormRepository class to insert.</param>
        object Add(dynamic form);

        /// <summary>
        ///     Inserts or updates multiple instances of dynamic class to IFormRepository.;
        /// </summary>
        /// <param name="forms">List of IFormRepository instances to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> forms);


        /// <summary>
        ///     Updates IFormRepository with an instance of dynamic class against the primary key value.
        /// </summary>
        /// <param name="form">The instance of IFormRepository class to update.</param>
        /// <param name="primaryKey">The value of the primary key which will be updated.</param>
        void Update(dynamic form, object primaryKey);

        /// <summary>
        ///     Deletes the item from  IFormRepository against the primary key value.
        /// </summary>
        /// <param name="primaryKey">The value of the primary key which will be deleted.</param>
        void Delete(object primaryKey);

        /// <summary>
        ///     Produces a paginated result of 50 IFormRepository classes.
        /// </summary>
        /// <returns>Returns the first page of collection of IFormRepository class.</returns>
        IEnumerable<dynamic> GetPaginatedResult();

        /// <summary>
        ///     Produces a paginated result of 50 IFormRepository classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of IFormRepository class.</returns>
        IEnumerable<dynamic> GetPaginatedResult(long pageNumber);

        List<Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        ///     Performs a filtered count on IFormRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of IFormRepository class using the filter.</returns>
        long CountWhere(List<Filter> filters);

        /// <summary>
        ///     Performs a filtered pagination against IFormRepository producing result of 50 items.
        /// </summary>
        /// <param name="pageNumber">
        ///     Enter the page number to produce the paginated result. If you provide a negative number, the
        ///     result will not be paginated.
        /// </param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of IFormRepository class.</returns>
        IEnumerable<dynamic> GetWhere(long pageNumber, List<Filter> filters);

        /// <summary>
        ///     Performs a filtered count on IFormRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of IFormRepository class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        ///     Gets a filtered result of IFormRepository producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">
        ///     Enter the page number to produce the paginated result. If you provide a negative number, the
        ///     result will not be paginated.
        /// </param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of IFormRepository class.</returns>
        IEnumerable<dynamic> GetFiltered(long pageNumber, string filterName);
    }
}