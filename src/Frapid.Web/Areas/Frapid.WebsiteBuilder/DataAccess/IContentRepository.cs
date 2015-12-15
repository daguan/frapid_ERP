// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IContentRepository
    {
        /// <summary>
        /// Counts the number of Content in IContentRepository.
        /// </summary>
        /// <returns>Returns the count of IContentRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Content. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Content.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetAll();

        /// <summary>
        /// Returns all instances of Content to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Content.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Content against contentId. 
        /// </summary>
        /// <param name="contentId">The column "content_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Content.</returns>
        Frapid.WebsiteBuilder.Entities.Content Get(int contentId);

        /// <summary>
        /// Gets the first record of Content.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Content.</returns>
        Frapid.WebsiteBuilder.Entities.Content GetFirst();

        /// <summary>
        /// Gets the previous record of Content sorted by contentId. 
        /// </summary>
        /// <param name="contentId">The column "content_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Content.</returns>
        Frapid.WebsiteBuilder.Entities.Content GetPrevious(int contentId);

        /// <summary>
        /// Gets the next record of Content sorted by contentId. 
        /// </summary>
        /// <param name="contentId">The column "content_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Content.</returns>
        Frapid.WebsiteBuilder.Entities.Content GetNext(int contentId);

        /// <summary>
        /// Gets the last record of Content.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Content.</returns>
        Frapid.WebsiteBuilder.Entities.Content GetLast();

        /// <summary>
        /// Returns multiple instances of the Content against contentIds. 
        /// </summary>
        /// <param name="contentIds">Array of column "content_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Content.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> Get(int[] contentIds);

        /// <summary>
        /// Custom fields are user defined form elements for IContentRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Content.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Content.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Content.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Content class to IContentRepository.
        /// </summary>
        /// <param name="content">The instance of Content class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic content, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Content class to IContentRepository.
        /// </summary>
        /// <param name="content">The instance of Content class to insert.</param>
        object Add(dynamic content);

        /// <summary>
        /// Inserts or updates multiple instances of Content class to IContentRepository.;
        /// </summary>
        /// <param name="contents">List of Content class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> contents);


        /// <summary>
        /// Updates IContentRepository with an instance of Content class against the primary key value.
        /// </summary>
        /// <param name="content">The instance of Content class to update.</param>
        /// <param name="contentId">The value of the column "content_id" which will be updated.</param>
        void Update(dynamic content, int contentId);

        /// <summary>
        /// Deletes Content from  IContentRepository against the primary key value.
        /// </summary>
        /// <param name="contentId">The value of the column "content_id" which will be deleted.</param>
        void Delete(int contentId);

        /// <summary>
        /// Produces a paginated result of 10 Content classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Content class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Content classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Content class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IContentRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Content class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IContentRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Content class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IContentRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Content class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IContentRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Content class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Content> GetFiltered(long pageNumber, string filterName);



    }
}