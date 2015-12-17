// ReSharper disable All
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.Framework.Extensions;
using Npgsql;
using Frapid.NPoco;
using Serilog;

namespace Frapid.WebsiteBuilder.DataAccess
{
    /// <summary>
    /// Provides simplified data access features to perform SCRUD operation on the database table "website.menu_items".
    /// </summary>
    public class MenuItem : DbAccess, IMenuItemRepository
    {
        /// <summary>
        /// The schema of this table. Returns literal "website".
        /// </summary>
        public override string _ObjectNamespace => "website";

        /// <summary>
        /// The schema unqualified name of this table. Returns literal "menu_items".
        /// </summary>
        public override string _ObjectName => "menu_items";

        /// <summary>
        /// Login id of application user accessing this table.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this table.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// The name of the database on which queries are being executed to.
        /// </summary>
        public string _Catalog { get; set; }

        /// <summary>
        /// Performs SQL count on the table "website.menu_items".
        /// </summary>
        /// <returns>Returns the number of rows of the table "website.menu_items".</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public long Count()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to count entity \"MenuItem\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT COUNT(*) FROM website.menu_items;";
            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "website.menu_items" to return all instances of the "MenuItem" class. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetAll()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ExportData, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the export entity \"MenuItem\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "website.menu_items" to return all instances of the "MenuItem" class to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<dynamic> Export()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ExportData, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the export entity \"MenuItem\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id;";
            return Factory.Get<dynamic>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "website.menu_items" with a where filter on the column "menu_item_id" to return a single instance of the "MenuItem" class. 
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.WebsiteBuilder.Entities.MenuItem Get(int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the get entity \"MenuItem\" filtered by \"MenuItemId\" with value {MenuItemId} was denied to the user with Login ID {_LoginId}", menuItemId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items WHERE menu_item_id=@0;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql, menuItemId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first record of the table "website.menu_items". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.WebsiteBuilder.Entities.MenuItem GetFirst()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the get the first record of entity \"MenuItem\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id LIMIT 1;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Gets the previous record of the table "website.menu_items" sorted by menuItemId.
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.WebsiteBuilder.Entities.MenuItem GetPrevious(int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the get the previous entity of \"MenuItem\" by \"MenuItemId\" with value {MenuItemId} was denied to the user with Login ID {_LoginId}", menuItemId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items WHERE menu_item_id < @0 ORDER BY menu_item_id DESC LIMIT 1;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql, menuItemId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the next record of the table "website.menu_items" sorted by menuItemId.
        /// </summary>
        /// <param name="menuItemId">The column "menu_item_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.WebsiteBuilder.Entities.MenuItem GetNext(int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the get the next entity of \"MenuItem\" by \"MenuItemId\" with value {MenuItemId} was denied to the user with Login ID {_LoginId}", menuItemId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items WHERE menu_item_id > @0 ORDER BY menu_item_id LIMIT 1;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql, menuItemId).FirstOrDefault();
        }


        /// <summary>
        /// Gets the last record of the table "website.menu_items". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.WebsiteBuilder.Entities.MenuItem GetLast()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the get the last record of entity \"MenuItem\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id DESC LIMIT 1;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Executes a select query on the table "website.menu_items" with a where filter on the column "menu_item_id" to return a multiple instances of the "MenuItem" class. 
        /// </summary>
        /// <param name="menuItemIds">Array of column "menu_item_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of "MenuItem" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> Get(int[] menuItemIds)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to entity \"MenuItem\" was denied to the user with Login ID {LoginId}. menuItemIds: {menuItemIds}.", this._LoginId, menuItemIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items WHERE menu_item_id IN (@0);";

            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql, menuItemIds);
        }

        /// <summary>
        /// Custom fields are user defined form elements for website.menu_items.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for the table website.menu_items</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.DataAccess.Models.CustomField> GetCustomFields(string resourceId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to get custom fields for entity \"MenuItem\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql;
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                sql = "SELECT * FROM config.custom_field_definition_view WHERE table_name='website.menu_items' ORDER BY field_order;";
                return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql);
            }

            sql = "SELECT * from config.get_custom_field_definition('website.menu_items'::text, @0::text) ORDER BY field_order;";
            return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql, resourceId);
        }

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding the row collection of website.menu_items.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for the table website.menu_items</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.DataAccess.Models.DisplayField> GetDisplayFields()
        {
            List<Frapid.DataAccess.Models.DisplayField> displayFields = new List<Frapid.DataAccess.Models.DisplayField>();

            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return displayFields;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to get display field for entity \"MenuItem\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT menu_item_id AS key, title as value FROM website.menu_items;";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                using (DataTable table = DbOperation.GetDataTable(this._Catalog, command))
                {
                    if (table?.Rows == null || table.Rows.Count == 0)
                    {
                        return displayFields;
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        if (row != null)
                        {
                            DisplayField displayField = new DisplayField
                            {
                                Key = row["key"].ToString(),
                                Value = row["value"].ToString()
                            };

                            displayFields.Add(displayField);
                        }
                    }
                }
            }

            return displayFields;
        }

        /// <summary>
        /// Inserts or updates the instance of MenuItem class on the database table "website.menu_items".
        /// </summary>
        /// <param name="menuItem">The instance of "MenuItem" class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object AddOrEdit(dynamic menuItem, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            menuItem.audit_user_id = this._UserId;
            menuItem.audit_ts = System.DateTime.UtcNow;

            object primaryKeyValue = menuItem.menu_item_id;

            if (Cast.To<int>(primaryKeyValue) > 0)
            {
                this.Update(menuItem, Cast.To<int>(primaryKeyValue));
            }
            else
            {
                primaryKeyValue = this.Add(menuItem);
            }

            string sql = "DELETE FROM config.custom_fields WHERE custom_field_setup_id IN(" +
                         "SELECT custom_field_setup_id " +
                         "FROM config.custom_field_setup " +
                         "WHERE form_name=config.get_custom_field_form_name('website.menu_items')" +
                         ");";

            Factory.NonQuery(this._Catalog, sql);

            if (customFields == null)
            {
                return primaryKeyValue;
            }

            foreach (var field in customFields)
            {
                sql = "INSERT INTO config.custom_fields(custom_field_setup_id, resource_id, value) " +
                      "SELECT config.get_custom_field_setup_id_by_table_name('website.menu_items', @0::character varying(100)), " +
                      "@1, @2;";

                Factory.NonQuery(this._Catalog, sql, field.FieldName, primaryKeyValue, field.Value);
            }

            return primaryKeyValue;
        }

        /// <summary>
        /// Inserts the instance of MenuItem class on the database table "website.menu_items".
        /// </summary>
        /// <param name="menuItem">The instance of "MenuItem" class to insert.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object Add(dynamic menuItem)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Create, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to add entity \"MenuItem\" was denied to the user with Login ID {LoginId}. {MenuItem}", this._LoginId, menuItem);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            return Factory.Insert(this._Catalog, menuItem, "website.menu_items", "menu_item_id");
        }

        /// <summary>
        /// Inserts or updates multiple instances of MenuItem class on the database table "website.menu_items";
        /// </summary>
        /// <param name="menuItems">List of "MenuItem" class to import.</param>
        /// <returns></returns>
        public List<object> BulkImport(List<ExpandoObject> menuItems)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ImportData, this._LoginId, this._Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to import entity \"MenuItem\" was denied to the user with Login ID {LoginId}. {menuItems}", this._LoginId, menuItems);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var result = new List<object>();
            int line = 0;
            try
            {
                using (Database db = new Database(ConnectionString.GetConnectionString(this._Catalog), Factory.ProviderName))
                {
                    using (ITransaction transaction = db.GetTransaction())
                    {
                        foreach (dynamic menuItem in menuItems)
                        {
                            line++;

                            menuItem.audit_user_id = this._UserId;
                            menuItem.audit_ts = System.DateTime.UtcNow;

                            object primaryKeyValue = menuItem.menu_item_id;

                            if (Cast.To<int>(primaryKeyValue) > 0)
                            {
                                result.Add(menuItem.menu_item_id);
                                db.Update("website.menu_items", "menu_item_id", menuItem, menuItem.menu_item_id);
                            }
                            else
                            {
                                result.Add(db.Insert("website.menu_items", "menu_item_id", menuItem));
                            }
                        }

                        transaction.Complete();
                    }

                    return result;
                }
            }
            catch (NpgsqlException ex)
            {
                string errorMessage = $"Error on line {line} ";

                if (ex.Code.StartsWith("P"))
                {
                    errorMessage += Factory.GetDbErrorResource(ex);

                    throw new DataAccessException(errorMessage, ex);
                }

                errorMessage += ex.Message;
                throw new DataAccessException(errorMessage, ex);
            }
            catch (System.Exception ex)
            {
                string errorMessage = $"Error on line {line} ";
                throw new DataAccessException(errorMessage, ex);
            }
        }

        /// <summary>
        /// Updates the row of the table "website.menu_items" with an instance of "MenuItem" class against the primary key value.
        /// </summary>
        /// <param name="menuItem">The instance of "MenuItem" class to update.</param>
        /// <param name="menuItemId">The value of the column "menu_item_id" which will be updated.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Update(dynamic menuItem, int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Edit, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to edit entity \"MenuItem\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}. {MenuItem}", menuItemId, this._LoginId, menuItem);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Factory.Update(this._Catalog, menuItem, menuItemId, "website.menu_items", "menu_item_id");
        }

        /// <summary>
        /// Deletes the row of the table "website.menu_items" against the primary key value.
        /// </summary>
        /// <param name="menuItemId">The value of the column "menu_item_id" which will be deleted.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Delete(int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Delete, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to delete entity \"MenuItem\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}.", menuItemId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "DELETE FROM website.menu_items WHERE menu_item_id=@0;";
            Factory.NonQuery(this._Catalog, sql, menuItemId);
        }

        /// <summary>
        /// Performs a select statement on table "website.menu_items" producing a paginated result of 10.
        /// </summary>
        /// <returns>Returns the first page of collection of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult()
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to the first page of the entity \"MenuItem\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id LIMIT 10 OFFSET 0;";
            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a select statement on table "website.menu_items" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetPaginatedResult(long pageNumber)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to Page #{Page} of the entity \"MenuItem\" was denied to the user with Login ID {LoginId}.", pageNumber, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 10;
            const string sql = "SELECT * FROM website.menu_items ORDER BY menu_item_id LIMIT 10 OFFSET @0;";

            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql, offset);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            const string sql = "SELECT * FROM config.filters WHERE object_name='website.menu_items' AND lower(filter_name)=lower(@0);";
            return Factory.Get<Frapid.DataAccess.Models.Filter>(catalog, sql, filterName).ToList();
        }

        /// <summary>
        /// Performs a filtered count on table "website.menu_items".
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "MenuItem" class using the filter.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public long CountWhere(List<Frapid.DataAccess.Models.Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to count entity \"MenuItem\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM website.menu_items WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.WebsiteBuilder.Entities.MenuItem(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "website.menu_items" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to Page #{Page} of the filtered entity \"MenuItem\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", pageNumber, this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 10;
            Sql sql = Sql.Builder.Append("SELECT * FROM website.menu_items WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.WebsiteBuilder.Entities.MenuItem(), filters);

            sql.OrderBy("menu_item_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 10);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered count on table "website.menu_items".
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "MenuItem" class using the filter.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public long CountFiltered(string filterName)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to count entity \"MenuItem\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);
            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM website.menu_items WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.WebsiteBuilder.Entities.MenuItem(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "website.menu_items" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "MenuItem" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.WebsiteBuilder.Entities.MenuItem> GetFiltered(long pageNumber, string filterName)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this._LoginId, this._Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to Page #{Page} of the filtered entity \"MenuItem\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", pageNumber, this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);

            long offset = (pageNumber - 1) * 10;
            Sql sql = Sql.Builder.Append("SELECT * FROM website.menu_items WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.WebsiteBuilder.Entities.MenuItem(), filters);

            sql.OrderBy("menu_item_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 10);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.WebsiteBuilder.Entities.MenuItem>(this._Catalog, sql);
        }


    }
}