// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Counts the number of Category in ICategoryRepository.
        /// </summary>
        /// <returns>Returns the count of ICategoryRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Category. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Category.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetAll();

        /// <summary>
        /// Returns all instances of Category to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Category.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Category against categoryId. 
        /// </summary>
        /// <param name="categoryId">The column "category_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Category.</returns>
        Frapid.WebsiteBuilder.Entities.Category Get(int categoryId);

        /// <summary>
        /// Gets the first record of Category.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Category.</returns>
        Frapid.WebsiteBuilder.Entities.Category GetFirst();

        /// <summary>
        /// Gets the previous record of Category sorted by categoryId. 
        /// </summary>
        /// <param name="categoryId">The column "category_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Category.</returns>
        Frapid.WebsiteBuilder.Entities.Category GetPrevious(int categoryId);

        /// <summary>
        /// Gets the next record of Category sorted by categoryId. 
        /// </summary>
        /// <param name="categoryId">The column "category_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Category.</returns>
        Frapid.WebsiteBuilder.Entities.Category GetNext(int categoryId);

        /// <summary>
        /// Gets the last record of Category.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Category.</returns>
        Frapid.WebsiteBuilder.Entities.Category GetLast();

        /// <summary>
        /// Returns multiple instances of the Category against categoryIds. 
        /// </summary>
        /// <param name="categoryIds">Array of column "category_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Category.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> Get(int[] categoryIds);

        /// <summary>
        /// Custom fields are user defined form elements for ICategoryRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Category.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Category.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Category.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Category class to ICategoryRepository.
        /// </summary>
        /// <param name="category">The instance of Category class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic category, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Category class to ICategoryRepository.
        /// </summary>
        /// <param name="category">The instance of Category class to insert.</param>
        object Add(dynamic category);

        /// <summary>
        /// Inserts or updates multiple instances of Category class to ICategoryRepository.;
        /// </summary>
        /// <param name="categories">List of Category class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> categories);


        /// <summary>
        /// Updates ICategoryRepository with an instance of Category class against the primary key value.
        /// </summary>
        /// <param name="category">The instance of Category class to update.</param>
        /// <param name="categoryId">The value of the column "category_id" which will be updated.</param>
        void Update(dynamic category, int categoryId);

        /// <summary>
        /// Deletes Category from  ICategoryRepository against the primary key value.
        /// </summary>
        /// <param name="categoryId">The value of the column "category_id" which will be deleted.</param>
        void Delete(int categoryId);

        /// <summary>
        /// Produces a paginated result of 10 Category classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Category class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Category classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Category class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ICategoryRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Category class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against ICategoryRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Category class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ICategoryRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Category class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of ICategoryRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Category class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Category> GetFiltered(long pageNumber, string filterName);



    }
}