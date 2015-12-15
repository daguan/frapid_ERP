// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IMenuItemRepository
    {
        /// <summary>
        /// Counts the number of MenuItem in IMenuItemRepository.
        /// </summary>
        /// <returns>Returns the count of IMenuItemRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of MenuItem. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of MenuItem.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetAll();

        /// <summary>
        /// Returns all instances of MenuItem to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of MenuItem.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the MenuItem against menuItemId. 
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of MenuItem.</returns>
        Frapid.WebsiteBuilder.Entities.MenuItem Get(int menuItemId);

        /// <summary>
        /// Gets the first record of MenuItem.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of MenuItem.</returns>
        Frapid.WebsiteBuilder.Entities.MenuItem GetFirst();

        /// <summary>
        /// Gets the previous record of MenuItem sorted by menuItemId. 
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of MenuItem.</returns>
        Frapid.WebsiteBuilder.Entities.MenuItem GetPrevious(int menuItemId);

        /// <summary>
        /// Gets the next record of MenuItem sorted by menuItemId. 
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of MenuItem.</returns>
        Frapid.WebsiteBuilder.Entities.MenuItem GetNext(int menuItemId);

        /// <summary>
        /// Gets the last record of MenuItem.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of MenuItem.</returns>
        Frapid.WebsiteBuilder.Entities.MenuItem GetLast();

        /// <summary>
        /// Returns multiple instances of the MenuItem against menuItemIds. 
        /// </summary>
        /// <param name="menuItemIds">Array of column "menu_item_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of MenuItem.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> Get(int[] menuItemIds);

        /// <summary>
        /// Custom fields are user defined form elements for IMenuItemRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for MenuItem.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding MenuItem.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for MenuItem.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of MenuItem class to IMenuItemRepository.
        /// </summary>
        /// <param name="menuItem">The instance of MenuItem class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic menuItem, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of MenuItem class to IMenuItemRepository.
        /// </summary>
        /// <param name="menuItem">The instance of MenuItem class to insert.</param>
        object Add(dynamic menuItem);

        /// <summary>
        /// Inserts or updates multiple instances of MenuItem class to IMenuItemRepository.;
        /// </summary>
        /// <param name="menuItems">List of MenuItem class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> menuItems);


        /// <summary>
        /// Updates IMenuItemRepository with an instance of MenuItem class against the primary key value.
        /// </summary>
        /// <param name="menuItem">The instance of MenuItem class to update.</param>
        /// <param name="menuItemId">The value of the column "menu_item_id" which will be updated.</param>
        void Update(dynamic menuItem, int menuItemId);

        /// <summary>
        /// Deletes MenuItem from  IMenuItemRepository against the primary key value.
        /// </summary>
        /// <param name="menuItemId">The value of the column "menu_item_id" which will be deleted.</param>
        void Delete(int menuItemId);

        /// <summary>
        /// Produces a paginated result of 10 MenuItem classes.
        /// </summary>
        /// <returns>Returns the first page of collection of MenuItem class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 MenuItem classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of MenuItem class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IMenuItemRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of MenuItem class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IMenuItemRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of MenuItem class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IMenuItemRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of MenuItem class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IMenuItemRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of MenuItem class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetFiltered(long pageNumber, string filterName);



    }
}