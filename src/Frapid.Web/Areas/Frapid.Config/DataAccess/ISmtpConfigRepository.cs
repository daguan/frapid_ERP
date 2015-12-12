// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.Config.DataAccess
{
    public interface ISmtpConfigRepository
    {
        /// <summary>
        /// Counts the number of SmtpConfig in ISmtpConfigRepository.
        /// </summary>
        /// <returns>Returns the count of ISmtpConfigRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of SmtpConfig. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of SmtpConfig.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> GetAll();

        /// <summary>
        /// Returns all instances of SmtpConfig to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of SmtpConfig.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the SmtpConfig against smtpId. 
        /// </summary>
        /// <param name="smtpId">The column "smtp_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of SmtpConfig.</returns>
        Frapid.Config.Entities.SmtpConfig Get(int smtpId);

        /// <summary>
        /// Gets the first record of SmtpConfig.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of SmtpConfig.</returns>
        Frapid.Config.Entities.SmtpConfig GetFirst();

        /// <summary>
        /// Gets the previous record of SmtpConfig sorted by smtpId. 
        /// </summary>
        /// <param name="smtpId">The column "smtp_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of SmtpConfig.</returns>
        Frapid.Config.Entities.SmtpConfig GetPrevious(int smtpId);

        /// <summary>
        /// Gets the next record of SmtpConfig sorted by smtpId. 
        /// </summary>
        /// <param name="smtpId">The column "smtp_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of SmtpConfig.</returns>
        Frapid.Config.Entities.SmtpConfig GetNext(int smtpId);

        /// <summary>
        /// Gets the last record of SmtpConfig.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of SmtpConfig.</returns>
        Frapid.Config.Entities.SmtpConfig GetLast();

        /// <summary>
        /// Returns multiple instances of the SmtpConfig against smtpIds. 
        /// </summary>
        /// <param name="smtpIds">Array of column "smtp_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of SmtpConfig.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> Get(int[] smtpIds);

        /// <summary>
        /// Custom fields are user defined form elements for ISmtpConfigRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for SmtpConfig.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding SmtpConfig.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for SmtpConfig.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of SmtpConfig class to ISmtpConfigRepository.
        /// </summary>
        /// <param name="smtpConfig">The instance of SmtpConfig class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic smtpConfig, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of SmtpConfig class to ISmtpConfigRepository.
        /// </summary>
        /// <param name="smtpConfig">The instance of SmtpConfig class to insert.</param>
        object Add(dynamic smtpConfig);

        /// <summary>
        /// Inserts or updates multiple instances of SmtpConfig class to ISmtpConfigRepository.;
        /// </summary>
        /// <param name="smtpConfigs">List of SmtpConfig class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> smtpConfigs);


        /// <summary>
        /// Updates ISmtpConfigRepository with an instance of SmtpConfig class against the primary key value.
        /// </summary>
        /// <param name="smtpConfig">The instance of SmtpConfig class to update.</param>
        /// <param name="smtpId">The value of the column "smtp_id" which will be updated.</param>
        void Update(dynamic smtpConfig, int smtpId);

        /// <summary>
        /// Deletes SmtpConfig from  ISmtpConfigRepository against the primary key value.
        /// </summary>
        /// <param name="smtpId">The value of the column "smtp_id" which will be deleted.</param>
        void Delete(int smtpId);

        /// <summary>
        /// Produces a paginated result of 10 SmtpConfig classes.
        /// </summary>
        /// <returns>Returns the first page of collection of SmtpConfig class.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 SmtpConfig classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of SmtpConfig class.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on ISmtpConfigRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of SmtpConfig class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against ISmtpConfigRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of SmtpConfig class.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on ISmtpConfigRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of SmtpConfig class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of ISmtpConfigRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of SmtpConfig class.</returns>
        IEnumerable<Frapid.Config.Entities.SmtpConfig> GetFiltered(long pageNumber, string filterName);



    }
}