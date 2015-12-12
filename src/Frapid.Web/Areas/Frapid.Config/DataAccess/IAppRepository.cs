// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface IAppRepository
    {
        /// <summary>
        /// Counts the number of App in IAppRepository.
        /// </summary>
        /// <returns>Returns the count of IAppRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of App. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of App.</returns>
        IEnumerable<Frapid.Config.Entities.App> GetAll();

        /// <summary>
        /// Returns all instances of App to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of App.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the App against appName. 
        /// </summary>
        /// <param name="appName">The column "app_name" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of App.</returns>
        Frapid.Config.Entities.App Get(string appName);

        /// <summary>
        /// Gets the first record of App.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of App.</returns>
        Frapid.Config.Entities.App GetFirst();

        /// <summary>
        /// Gets the previous record of App sorted by appName. 
        /// </summary>
        /// <param name="appName">The column "app_name" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of App.</returns>
        Frapid.Config.Entities.App GetPrevious(string appName);

        /// <summary>
        /// Gets the next record of App sorted by appName. 
        /// </summary>
        /// <param name="appName">The column "app_name" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of App.</returns>
        Frapid.Config.Entities.App GetNext(string appName);

        /// <summary>
        /// Gets the last record of App.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of App.</returns>
        Frapid.Config.Entities.App GetLast();

        /// <summary>
        /// Returns multiple instances of the App against appNames. 
        /// </summary>
        /// <param name="appNames">Array of column "app_name" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of App.</returns>
        IEnumerable<Frapid.Config.Entities.App> Get(string[] appNames);

        /// <summary>
        /// Custom fields are user defined form elements for IAppRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for App.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding App.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for App.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of App class to IAppRepository.
        /// </summary>
        /// <param name="app">The instance of App class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic app, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of App class to IAppRepository.
        /// </summary>
        /// <param name="app">The instance of App class to insert.</param>
        object Add(dynamic app);

        /// <summary>
        /// Inserts or updates multiple instances of App class to IAppRepository.;
        /// </summary>
        /// <param name="apps">List of App class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> apps);


        /// <summary>
        /// Updates IAppRepository with an instance of App class against the primary key value.
        /// </summary>
        /// <param name="app">The instance of App class to update.</param>
        /// <param name="appName">The value of the column "app_name" which will be updated.</param>
        void Update(dynamic app, string appName);

        /// <summary>
        /// Deletes App from  IAppRepository against the primary key value.
        /// </summary>
        /// <param name="appName">The value of the column "app_name" which will be deleted.</param>
        void Delete(string appName);

        /// <summary>
        /// Produces a paginated result of 10 App classes.
        /// </summary>
        /// <returns>Returns the first page of collection of App class.</returns>
        IEnumerable<Frapid.Config.Entities.App> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 App classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of App class.</returns>
        IEnumerable<Frapid.Config.Entities.App> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IAppRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of App class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IAppRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of App class.</returns>
        IEnumerable<Frapid.Config.Entities.App> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IAppRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of App class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IAppRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of App class.</returns>
        IEnumerable<Frapid.Config.Entities.App> GetFiltered(long pageNumber, string filterName);



    }
}