// ReSharper disable All
using System.Collections.Generic;
using System.Dynamic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;

namespace Frapid.WebsiteBuilder.DataAccess
{
    public interface IContactRepository
    {
        /// <summary>
        /// Counts the number of Contact in IContactRepository.
        /// </summary>
        /// <returns>Returns the count of IContactRepository.</returns>
        long Count();

        /// <summary>
        /// Returns all instances of Contact. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Contact.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetAll();

        /// <summary>
        /// Returns all instances of Contact to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of Contact.</returns>
        IEnumerable<dynamic> Export();

        /// <summary>
        /// Returns a single instance of the Contact against contactId. 
        /// </summary>
        /// <param name="contactId">The column "contact_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of Contact.</returns>
        Frapid.WebsiteBuilder.Entities.Contact Get(int contactId);

        /// <summary>
        /// Gets the first record of Contact.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Contact.</returns>
        Frapid.WebsiteBuilder.Entities.Contact GetFirst();

        /// <summary>
        /// Gets the previous record of Contact sorted by contactId. 
        /// </summary>
        /// <param name="contactId">The column "contact_id" parameter used to find the previous record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Contact.</returns>
        Frapid.WebsiteBuilder.Entities.Contact GetPrevious(int contactId);

        /// <summary>
        /// Gets the next record of Contact sorted by contactId. 
        /// </summary>
        /// <param name="contactId">The column "contact_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of Contact.</returns>
        Frapid.WebsiteBuilder.Entities.Contact GetNext(int contactId);

        /// <summary>
        /// Gets the last record of Contact.
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of Contact.</returns>
        Frapid.WebsiteBuilder.Entities.Contact GetLast();

        /// <summary>
        /// Returns multiple instances of the Contact against contactIds. 
        /// </summary>
        /// <param name="contactIds">Array of column "contact_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of Contact.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> Get(int[] contactIds);

        /// <summary>
        /// Custom fields are user defined form elements for IContactRepository.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for Contact.</returns>
        IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId);

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding Contact.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for Contact.</returns>
        IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields();

        /// <summary>
        /// Inserts the instance of Contact class to IContactRepository.
        /// </summary>
        /// <param name="contact">The instance of Contact class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        object AddOrEdit(dynamic contact, List<Frapid.DataAccess.Models.CustomField> customFields);

        /// <summary>
        /// Inserts the instance of Contact class to IContactRepository.
        /// </summary>
        /// <param name="contact">The instance of Contact class to insert.</param>
        object Add(dynamic contact);

        /// <summary>
        /// Inserts or updates multiple instances of Contact class to IContactRepository.;
        /// </summary>
        /// <param name="contacts">List of Contact class to import.</param>
        /// <returns>Returns list of inserted object ids.</returns>
        List<object> BulkImport(List<ExpandoObject> contacts);


        /// <summary>
        /// Updates IContactRepository with an instance of Contact class against the primary key value.
        /// </summary>
        /// <param name="contact">The instance of Contact class to update.</param>
        /// <param name="contactId">The value of the column "contact_id" which will be updated.</param>
        void Update(dynamic contact, int contactId);

        /// <summary>
        /// Deletes Contact from  IContactRepository against the primary key value.
        /// </summary>
        /// <param name="contactId">The value of the column "contact_id" which will be deleted.</param>
        void Delete(int contactId);

        /// <summary>
        /// Produces a paginated result of 10 Contact classes.
        /// </summary>
        /// <returns>Returns the first page of collection of Contact class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetPaginatedResult();

        /// <summary>
        /// Produces a paginated result of 10 Contact classes.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of Contact class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetPaginatedResult(long pageNumber);

        List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName);

        /// <summary>
        /// Performs a filtered count on IContactRepository.
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of Contact class using the filter.</returns>
        long CountWhere(List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered pagination against IContactRepository producing result of 10 items.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of Contact class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters);

        /// <summary>
        /// Performs a filtered count on IContactRepository.
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of Contact class using the filter.</returns>
        long CountFiltered(string filterName);

        /// <summary>
        /// Gets a filtered result of IContactRepository producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of Contact class.</returns>
        IEnumerable<Frapid.WebsiteBuilder.Entities.Contact> GetFiltered(long pageNumber, string filterName);



    }
}