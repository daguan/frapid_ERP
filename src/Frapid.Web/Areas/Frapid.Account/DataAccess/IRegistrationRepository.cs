// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IRegistrationRepository
    {
        /// <summary>
        /// Counts the number of Registration in IRegistrationRepository.
        /// </summary>
        /// <returns>Returns the count of IRegistrationRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Registration. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Registration.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> GetAll();

        /// <summary>
        /// Returns all instances of Registration to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Registration.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Registration against registrationId. 
        /// </summary>
        /// <param name="registrationId">The column "registration_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Registration.</returns>
        Frapid.Account.Entities.Registration Get(System.Guid registrationId);

        /// <summary>
        /// Gets the first record of Registration.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Registration.</returns>
        Frapid.Account.Entities.Registration GetFirst();

        /// <summary>
        /// Gets the previous record of Registration sorted by registrationId. 
        /// </summary>
        /// <param name="registrationId">The column "registration_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Registration.</returns>
        Frapid.Account.Entities.Registration GetPrevious(System.Guid registrationId);

        /// <summary>
        /// Gets the next record of Registration sorted by registrationId. 
        /// </summary>
        /// <param name="registrationId">The column "registration_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Registration.</returns>
        Frapid.Account.Entities.Registration GetNext(System.Guid registrationId);

        /// <summary>
        /// Gets the last record of Registration.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Registration.</returns>
        Frapid.Account.Entities.Registration GetLast();

        /// <summary>
        /// Returns multiple instances of the Registration against registrationIds. 
        /// </summary>
        /// <param name="registrationIds">Array of column "registration_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Registration.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> Get(System.Guid[] registrationIds);

        /// <summary>
        /// Custom fields are user defined form elements for IRegistrationRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Registration.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Registration.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Registration.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Registration class to IRegistrationRepository.
        /// </summary>
        /// <param name="registration">The instance of Registration class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic registration, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Registration class to IRegistrationRepository.
        /// </summary>
        /// <param name="registration">The instance of Registration class to insert.</param>
        object Add(dynamic registration);

        /// <summary>
        /// Inserts or updates multiple instances of Registration class to IRegistrationRepository.;
        /// </summary>
        /// <param name="registrations">List of Registration class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> registrations);


        /// <summary>
        /// Updates IRegistrationRepository with an instance of Registration class against the primary key value.
        /// </summary>
        /// <param name="registration">The instance of Registration class to update.</param>
        /// <param name="registrationId">The value of the column "registration_id" which will be updated.</param>
        void Update(dynamic registration, System.Guid registrationId);

        /// <summary>
        /// Deletes Registration from  IRegistrationRepository against the primary key value.
        /// </summary>
        /// <param name="registrationId">The value of the column "registration_id" which will be deleted.</param>
        void Delete(System.Guid registrationId);

        /// <summary>
        /// Produces a paginated result of 10 Registration classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Registration class.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Registration classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Registration class.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IRegistrationRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Registration class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IRegistrationRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Registration class.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IRegistrationRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Registration class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IRegistrationRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Registration class.</returns>
        IEnumerable<Frapid.Account.Entities.Registration> GetFiltered(long pageNumber, string filterName);



    }
}