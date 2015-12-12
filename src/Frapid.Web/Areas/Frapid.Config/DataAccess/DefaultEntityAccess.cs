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

namespace Frapid.Config.DataAccess
{
    /// <summary>
    /// Provides simplified data access features to perform SCRUD operation on the database table "config.default_entity_access".
    /// </summary>
    public class DefaultEntityAccess : DbAccess, IDefaultEntityAccessRepository
    {
        /// <summary>
        /// The schema of this table. Returns literal "config".
        /// </summary>
        public override string _ObjectNamespace => "config";

        /// <summary>
        /// The schema unqualified name of this table. Returns literal "default_entity_access".
        /// </summary>
        public override string _ObjectName => "default_entity_access";

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
        /// Performs SQL count on the table "config.default_entity_access".
        /// </summary>
        /// <returns>Returns the number of rows of the table "config.default_entity_access".</returns>
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
                    Log.Information("Access to count entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT COUNT(*) FROM config.default_entity_access;";
            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "config.default_entity_access" to return all instances of the "DefaultEntityAccess" class. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "DefaultEntityAccess" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetAll()
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
                    Log.Information("Access to the export entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "config.default_entity_access" to return all instances of the "DefaultEntityAccess" class to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "DefaultEntityAccess" class.</returns>
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
                    Log.Information("Access to the export entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id;";
            return Factory.Get<dynamic>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "config.default_entity_access" with a where filter on the column "default_entity_access_id" to return a single instance of the "DefaultEntityAccess" class. 
        /// </summary>
        /// <param name="defaultEntityAccessId">The column "default_entity_access_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Config.Entities.DefaultEntityAccess Get(int defaultEntityAccessId)
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
                    Log.Information("Access to the get entity \"DefaultEntityAccess\" filtered by \"DefaultEntityAccessId\" with value {DefaultEntityAccessId} was denied to the user with Login ID {_LoginId}", defaultEntityAccessId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access WHERE default_entity_access_id=@0;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql, defaultEntityAccessId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first record of the table "config.default_entity_access". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Config.Entities.DefaultEntityAccess GetFirst()
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
                    Log.Information("Access to the get the first record of entity \"DefaultEntityAccess\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id LIMIT 1;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Gets the previous record of the table "config.default_entity_access" sorted by defaultEntityAccessId.
        /// </summary>
        /// <param name="defaultEntityAccessId">The column "default_entity_access_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Config.Entities.DefaultEntityAccess GetPrevious(int defaultEntityAccessId)
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
                    Log.Information("Access to the get the previous entity of \"DefaultEntityAccess\" by \"DefaultEntityAccessId\" with value {DefaultEntityAccessId} was denied to the user with Login ID {_LoginId}", defaultEntityAccessId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access WHERE default_entity_access_id < @0 ORDER BY default_entity_access_id DESC LIMIT 1;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql, defaultEntityAccessId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the next record of the table "config.default_entity_access" sorted by defaultEntityAccessId.
        /// </summary>
        /// <param name="defaultEntityAccessId">The column "default_entity_access_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Config.Entities.DefaultEntityAccess GetNext(int defaultEntityAccessId)
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
                    Log.Information("Access to the get the next entity of \"DefaultEntityAccess\" by \"DefaultEntityAccessId\" with value {DefaultEntityAccessId} was denied to the user with Login ID {_LoginId}", defaultEntityAccessId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access WHERE default_entity_access_id > @0 ORDER BY default_entity_access_id LIMIT 1;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql, defaultEntityAccessId).FirstOrDefault();
        }


        /// <summary>
        /// Gets the last record of the table "config.default_entity_access". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Config.Entities.DefaultEntityAccess GetLast()
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
                    Log.Information("Access to the get the last record of entity \"DefaultEntityAccess\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id DESC LIMIT 1;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Executes a select query on the table "config.default_entity_access" with a where filter on the column "default_entity_access_id" to return a multiple instances of the "DefaultEntityAccess" class. 
        /// </summary>
        /// <param name="defaultEntityAccessIds">Array of column "default_entity_access_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of "DefaultEntityAccess" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> Get(int[] defaultEntityAccessIds)
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
                    Log.Information("Access to entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. defaultEntityAccessIds: {defaultEntityAccessIds}.", this._LoginId, defaultEntityAccessIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access WHERE default_entity_access_id IN (@0);";

            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql, defaultEntityAccessIds);
        }

        /// <summary>
        /// Custom fields are user defined form elements for config.default_entity_access.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for the table config.default_entity_access</returns>
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
                    Log.Information("Access to get custom fields for entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql;
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                sql = "SELECT * FROM config.custom_field_definition_view WHERE table_name='config.default_entity_access' ORDER BY field_order;";
                return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql);
            }

            sql = "SELECT * from config.get_custom_field_definition('config.default_entity_access'::text, @0::text) ORDER BY field_order;";
            return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql, resourceId);
        }

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding the row collection of config.default_entity_access.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for the table config.default_entity_access</returns>
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
                    Log.Information("Access to get display field for entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT default_entity_access_id AS key, entity_name as value FROM config.default_entity_access;";
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
        /// Inserts or updates the instance of DefaultEntityAccess class on the database table "config.default_entity_access".
        /// </summary>
        /// <param name="defaultEntityAccess">The instance of "DefaultEntityAccess" class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object AddOrEdit(dynamic defaultEntityAccess, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }

            defaultEntityAccess.audit_user_id = this._UserId;
            defaultEntityAccess.audit_ts = System.DateTime.UtcNow;

            object primaryKeyValue = defaultEntityAccess.default_entity_access_id;

            if (Cast.To<int>(primaryKeyValue) > 0)
            {
                this.Update(defaultEntityAccess, Cast.To<int>(primaryKeyValue));
            }
            else
            {
                primaryKeyValue = this.Add(defaultEntityAccess);
            }

            string sql = "DELETE FROM config.custom_fields WHERE custom_field_setup_id IN(" +
                         "SELECT custom_field_setup_id " +
                         "FROM config.custom_field_setup " +
                         "WHERE form_name=config.get_custom_field_form_name('config.default_entity_access')" +
                         ");";

            Factory.NonQuery(this._Catalog, sql);

            if (customFields == null)
            {
                return primaryKeyValue;
            }

            foreach (var field in customFields)
            {
                sql = "INSERT INTO config.custom_fields(custom_field_setup_id, resource_id, value) " +
                      "SELECT config.get_custom_field_setup_id_by_table_name('config.default_entity_access', @0::character varying(100)), " +
                      "@1, @2;";

                Factory.NonQuery(this._Catalog, sql, field.FieldName, primaryKeyValue, field.Value);
            }

            return primaryKeyValue;
        }

        /// <summary>
        /// Inserts the instance of DefaultEntityAccess class on the database table "config.default_entity_access".
        /// </summary>
        /// <param name="defaultEntityAccess">The instance of "DefaultEntityAccess" class to insert.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object Add(dynamic defaultEntityAccess)
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
                    Log.Information("Access to add entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. {DefaultEntityAccess}", this._LoginId, defaultEntityAccess);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            return Factory.Insert(this._Catalog, defaultEntityAccess, "config.default_entity_access", "default_entity_access_id");
        }

        /// <summary>
        /// Inserts or updates multiple instances of DefaultEntityAccess class on the database table "config.default_entity_access";
        /// </summary>
        /// <param name="defaultEntityAccesses">List of "DefaultEntityAccess" class to import.</param>
        /// <returns></returns>
        public List<object> BulkImport(List<ExpandoObject> defaultEntityAccesses)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ImportData, this._LoginId, this._Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to import entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. {defaultEntityAccesses}", this._LoginId, defaultEntityAccesses);
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
                        foreach (dynamic defaultEntityAccess in defaultEntityAccesses)
                        {
                            line++;

                            defaultEntityAccess.audit_user_id = this._UserId;
                            defaultEntityAccess.audit_ts = System.DateTime.UtcNow;

                            object primaryKeyValue = defaultEntityAccess.default_entity_access_id;

                            if (Cast.To<int>(primaryKeyValue) > 0)
                            {
                                result.Add(defaultEntityAccess.default_entity_access_id);
                                db.Update("config.default_entity_access", "default_entity_access_id", defaultEntityAccess, defaultEntityAccess.default_entity_access_id);
                            }
                            else
                            {
                                result.Add(db.Insert("config.default_entity_access", "default_entity_access_id", defaultEntityAccess));
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
        /// Updates the row of the table "config.default_entity_access" with an instance of "DefaultEntityAccess" class against the primary key value.
        /// </summary>
        /// <param name="defaultEntityAccess">The instance of "DefaultEntityAccess" class to update.</param>
        /// <param name="defaultEntityAccessId">The value of the column "default_entity_access_id" which will be updated.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Update(dynamic defaultEntityAccess, int defaultEntityAccessId)
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
                    Log.Information("Access to edit entity \"DefaultEntityAccess\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}. {DefaultEntityAccess}", defaultEntityAccessId, this._LoginId, defaultEntityAccess);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Factory.Update(this._Catalog, defaultEntityAccess, defaultEntityAccessId, "config.default_entity_access", "default_entity_access_id");
        }

        /// <summary>
        /// Deletes the row of the table "config.default_entity_access" against the primary key value.
        /// </summary>
        /// <param name="defaultEntityAccessId">The value of the column "default_entity_access_id" which will be deleted.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Delete(int defaultEntityAccessId)
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
                    Log.Information("Access to delete entity \"DefaultEntityAccess\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}.", defaultEntityAccessId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "DELETE FROM config.default_entity_access WHERE default_entity_access_id=@0;";
            Factory.NonQuery(this._Catalog, sql, defaultEntityAccessId);
        }

        /// <summary>
        /// Performs a select statement on table "config.default_entity_access" producing a paginated result of 10.
        /// </summary>
        /// <returns>Returns the first page of collection of "DefaultEntityAccess" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetPaginatedResult()
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
                    Log.Information("Access to the first page of the entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id LIMIT 10 OFFSET 0;";
            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a select statement on table "config.default_entity_access" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "DefaultEntityAccess" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetPaginatedResult(long pageNumber)
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
                    Log.Information("Access to Page #{Page} of the entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}.", pageNumber, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 10;
            const string sql = "SELECT * FROM config.default_entity_access ORDER BY default_entity_access_id LIMIT 10 OFFSET @0;";

            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql, offset);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            const string sql = "SELECT * FROM config.filters WHERE object_name='config.default_entity_access' AND lower(filter_name)=lower(@0);";
            return Factory.Get<Frapid.DataAccess.Models.Filter>(catalog, sql, filterName).ToList();
        }

        /// <summary>
        /// Performs a filtered count on table "config.default_entity_access".
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "DefaultEntityAccess" class using the filter.</returns>
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
                    Log.Information("Access to count entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM config.default_entity_access WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Config.Entities.DefaultEntityAccess(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "config.default_entity_access" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "DefaultEntityAccess" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
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
                    Log.Information("Access to Page #{Page} of the filtered entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", pageNumber, this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 10;
            Sql sql = Sql.Builder.Append("SELECT * FROM config.default_entity_access WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Config.Entities.DefaultEntityAccess(), filters);

            sql.OrderBy("default_entity_access_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 10);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered count on table "config.default_entity_access".
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "DefaultEntityAccess" class using the filter.</returns>
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
                    Log.Information("Access to count entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);
            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM config.default_entity_access WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Config.Entities.DefaultEntityAccess(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "config.default_entity_access" producing a paginated result of 10.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "DefaultEntityAccess" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Config.Entities.DefaultEntityAccess> GetFiltered(long pageNumber, string filterName)
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
                    Log.Information("Access to Page #{Page} of the filtered entity \"DefaultEntityAccess\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", pageNumber, this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);

            long offset = (pageNumber - 1) * 10;
            Sql sql = Sql.Builder.Append("SELECT * FROM config.default_entity_access WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Config.Entities.DefaultEntityAccess(), filters);

            sql.OrderBy("default_entity_access_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 10);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.Config.Entities.DefaultEntityAccess>(this._Catalog, sql);
        }


    }
}