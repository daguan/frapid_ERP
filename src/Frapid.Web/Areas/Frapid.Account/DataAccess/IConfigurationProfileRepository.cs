// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Account.DataAccess
{
    public interface IConfigurationProfileRepository
    {
        /// <summary>
        /// Counts the number of ConfigurationProfile in IConfigurationProfileRepository.
        /// </summary>
        /// <returns>Returns the count of IConfigurationProfileRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of ConfigurationProfile. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of ConfigurationProfile.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetAll();

        /// <summary>
        /// Returns all instances of ConfigurationProfile to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of ConfigurationProfile.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the ConfigurationProfile against profileId. 
        /// </summary>
        /// <param name="profileId">The column "profile_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of ConfigurationProfile.</returns>
        Frapid.Account.Entities.ConfigurationProfile Get(int profileId);

        /// <summary>
        /// Gets the first record of ConfigurationProfile.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of ConfigurationProfile.</returns>
        Frapid.Account.Entities.ConfigurationProfile GetFirst();

        /// <summary>
        /// Gets the previous record of ConfigurationProfile sorted by profileId. 
        /// </summary>
        /// <param name="profileId">The column "profile_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of ConfigurationProfile.</returns>
        Frapid.Account.Entities.ConfigurationProfile GetPrevious(int profileId);

        /// <summary>
        /// Gets the next record of ConfigurationProfile sorted by profileId. 
        /// </summary>
        /// <param name="profileId">The column "profile_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of ConfigurationProfile.</returns>
        Frapid.Account.Entities.ConfigurationProfile GetNext(int profileId);

        /// <summary>
        /// Gets the last record of ConfigurationProfile.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of ConfigurationProfile.</returns>
        Frapid.Account.Entities.ConfigurationProfile GetLast();

        /// <summary>
        /// Returns multiple instances of the ConfigurationProfile against profileIds. 
        /// </summary>
        /// <param name="profileIds">Array of column "profile_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of ConfigurationProfile.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> Get(int[] profileIds);

        /// <summary>
        /// Custom fields are user defined form elements for IConfigurationProfileRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for ConfigurationProfile.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding ConfigurationProfile.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for ConfigurationProfile.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of ConfigurationProfile class to IConfigurationProfileRepository.
        /// </summary>
        /// <param name="configurationProfile">The instance of ConfigurationProfile class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic configurationProfile, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of ConfigurationProfile class to IConfigurationProfileRepository.
        /// </summary>
        /// <param name="configurationProfile">The instance of ConfigurationProfile class to insert.</param>
        object Add(dynamic configurationProfile);

        /// <summary>
        /// Inserts or updates multiple instances of ConfigurationProfile class to IConfigurationProfileRepository.;
        /// </summary>
        /// <param name="configurationProfiles">List of ConfigurationProfile class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> configurationProfiles);


        /// <summary>
        /// Updates IConfigurationProfileRepository with an instance of ConfigurationProfile class against the primary key value.
        /// </summary>
        /// <param name="configurationProfile">The instance of ConfigurationProfile class to update.</param>
        /// <param name="profileId">The value of the column "profile_id" which will be updated.</param>
        void Update(dynamic configurationProfile, int profileId);

        /// <summary>
        /// Deletes ConfigurationProfile from  IConfigurationProfileRepository against the primary key value.
        /// </summary>
        /// <param name="profileId">The value of the column "profile_id" which will be deleted.</param>
        void Delete(int profileId);

        /// <summary>
        /// Produces a paginated result of 50 ConfigurationProfile classes.
        /// </summary>
        /// <returns>Returns the first page of collection of ConfigurationProfile class.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 50 ConfigurationProfile classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of ConfigurationProfile class.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IConfigurationProfileRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of ConfigurationProfile class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IConfigurationProfileRepository producing result of 50 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of ConfigurationProfile class.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IConfigurationProfileRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of ConfigurationProfile class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IConfigurationProfileRepository producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of ConfigurationProfile class.</returns>
        IEnumerable<Frapid.Account.Entities.ConfigurationProfile> GetFiltered(long pageNumber, string filterName);



    }
}