// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IInstalledDomainRepository
    {
        /// <summary>
        /// Counts the number of InstalledDomain in IInstalledDomainRepository.
        /// </summary>
        /// <returns>Returns the count of IInstalledDomainRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of InstalledDomain. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of InstalledDomain.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> GetAll();

        /// <summary>
        /// Returns all instances of InstalledDomain to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of InstalledDomain.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the InstalledDomain against domainId. 
        /// </summary>
        /// <param name="domainId">The column "domain_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of InstalledDomain.</returns>
        Frapid.Account.Entities.InstalledDomain Get(int domainId);

        /// <summary>
        /// Gets the first record of InstalledDomain.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of InstalledDomain.</returns>
        Frapid.Account.Entities.InstalledDomain GetFirst();

        /// <summary>
        /// Gets the previous record of InstalledDomain sorted by domainId. 
        /// </summary>
        /// <param name="domainId">The column "domain_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of InstalledDomain.</returns>
        Frapid.Account.Entities.InstalledDomain GetPrevious(int domainId);

        /// <summary>
        /// Gets the next record of InstalledDomain sorted by domainId. 
        /// </summary>
        /// <param name="domainId">The column "domain_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of InstalledDomain.</returns>
        Frapid.Account.Entities.InstalledDomain GetNext(int domainId);

        /// <summary>
        /// Gets the last record of InstalledDomain.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of InstalledDomain.</returns>
        Frapid.Account.Entities.InstalledDomain GetLast();

        /// <summary>
        /// Returns multiple instances of the InstalledDomain against domainIds. 
        /// </summary>
        /// <param name="domainIds">Array of column "domain_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of InstalledDomain.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> Get(int[] domainIds);

        /// <summary>
        /// Custom fields are user defined form elements for IInstalledDomainRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for InstalledDomain.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding InstalledDomain.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for InstalledDomain.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of InstalledDomain class to IInstalledDomainRepository.
        /// </summary>
        /// <param name="installedDomain">The instance of InstalledDomain class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic installedDomain, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of InstalledDomain class to IInstalledDomainRepository.
        /// </summary>
        /// <param name="installedDomain">The instance of InstalledDomain class to insert.</param>
        object Add(dynamic installedDomain);

        /// <summary>
        /// Inserts or updates multiple instances of InstalledDomain class to IInstalledDomainRepository.;
        /// </summary>
        /// <param name="installedDomains">List of InstalledDomain class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> installedDomains);


        /// <summary>
        /// Updates IInstalledDomainRepository with an instance of InstalledDomain class against the primary key value.
        /// </summary>
        /// <param name="installedDomain">The instance of InstalledDomain class to update.</param>
        /// <param name="domainId">The value of the column "domain_id" which will be updated.</param>
        void Update(dynamic installedDomain, int domainId);

        /// <summary>
        /// Deletes InstalledDomain from  IInstalledDomainRepository against the primary key value.
        /// </summary>
        /// <param name="domainId">The value of the column "domain_id" which will be deleted.</param>
        void Delete(int domainId);

        /// <summary>
        /// Produces a paginated result of 10 InstalledDomain classes.
        /// </summary>
        /// <returns>Returns the first page of collection of InstalledDomain class.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 InstalledDomain classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of InstalledDomain class.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IInstalledDomainRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of InstalledDomain class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IInstalledDomainRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of InstalledDomain class.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IInstalledDomainRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of InstalledDomain class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IInstalledDomainRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of InstalledDomain class.</returns>
        IEnumerable<Frapid.Account.Entities.InstalledDomain> GetFiltered(long pageNumber, string filterName);



    }
}