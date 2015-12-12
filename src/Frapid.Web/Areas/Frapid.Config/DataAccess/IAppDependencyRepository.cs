// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface IAppDependencyRepository
    {
        /// <summary>
        /// Counts the number of AppDependency in IAppDependencyRepository.
        /// </summary>
        /// <returns>Returns the count of IAppDependencyRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of AppDependency. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of AppDependency.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> GetAll();

        /// <summary>
        /// Returns all instances of AppDependency to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of AppDependency.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the AppDependency against appDependencyId. 
        /// </summary>
        /// <param name="appDependencyId">The column "app_dependency_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of AppDependency.</returns>
        Frapid.Config.Entities.AppDependency Get(int appDependencyId);

        /// <summary>
        /// Gets the first record of AppDependency.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of AppDependency.</returns>
        Frapid.Config.Entities.AppDependency GetFirst();

        /// <summary>
        /// Gets the previous record of AppDependency sorted by appDependencyId. 
        /// </summary>
        /// <param name="appDependencyId">The column "app_dependency_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of AppDependency.</returns>
        Frapid.Config.Entities.AppDependency GetPrevious(int appDependencyId);

        /// <summary>
        /// Gets the next record of AppDependency sorted by appDependencyId. 
        /// </summary>
        /// <param name="appDependencyId">The column "app_dependency_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of AppDependency.</returns>
        Frapid.Config.Entities.AppDependency GetNext(int appDependencyId);

        /// <summary>
        /// Gets the last record of AppDependency.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of AppDependency.</returns>
        Frapid.Config.Entities.AppDependency GetLast();

        /// <summary>
        /// Returns multiple instances of the AppDependency against appDependencyIds. 
        /// </summary>
        /// <param name="appDependencyIds">Array of column "app_dependency_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of AppDependency.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> Get(int[] appDependencyIds);

        /// <summary>
        /// Custom fields are user defined form elements for IAppDependencyRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for AppDependency.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding AppDependency.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for AppDependency.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of AppDependency class to IAppDependencyRepository.
        /// </summary>
        /// <param name="appDependency">The instance of AppDependency class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic appDependency, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of AppDependency class to IAppDependencyRepository.
        /// </summary>
        /// <param name="appDependency">The instance of AppDependency class to insert.</param>
        object Add(dynamic appDependency);

        /// <summary>
        /// Inserts or updates multiple instances of AppDependency class to IAppDependencyRepository.;
        /// </summary>
        /// <param name="appDependencies">List of AppDependency class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> appDependencies);


        /// <summary>
        /// Updates IAppDependencyRepository with an instance of AppDependency class against the primary key value.
        /// </summary>
        /// <param name="appDependency">The instance of AppDependency class to update.</param>
        /// <param name="appDependencyId">The value of the column "app_dependency_id" which will be updated.</param>
        void Update(dynamic appDependency, int appDependencyId);

        /// <summary>
        /// Deletes AppDependency from  IAppDependencyRepository against the primary key value.
        /// </summary>
        /// <param name="appDependencyId">The value of the column "app_dependency_id" which will be deleted.</param>
        void Delete(int appDependencyId);

        /// <summary>
        /// Produces a paginated result of 10 AppDependency classes.
        /// </summary>
        /// <returns>Returns the first page of collection of AppDependency class.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 AppDependency classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of AppDependency class.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IAppDependencyRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of AppDependency class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IAppDependencyRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of AppDependency class.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IAppDependencyRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of AppDependency class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IAppDependencyRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of AppDependency class.</returns>
        IEnumerable<Frapid.Config.Entities.AppDependency> GetFiltered(long pageNumber, string filterName);



    }
}