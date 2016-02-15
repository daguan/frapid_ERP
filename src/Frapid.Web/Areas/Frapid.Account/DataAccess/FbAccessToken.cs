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

namespace Frapid.Account.DataAccess
{
    /// <summary>
    /// Provides simplified data access features to perform SCRUD operation on the database table "account.fb_access_tokens".
    /// </summary>
    public class FbAccessToken : DbAccess, IFbAccessTokenRepository
    {
        /// <summary>
        /// The schema of this table. Returns literal "account".
        /// </summary>
        public override string _ObjectNamespace => "account";

        /// <summary>
        /// The schema unqualified name of this table. Returns literal "fb_access_tokens".
        /// </summary>
        public override string _ObjectName => "fb_access_tokens";

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
        /// Performs SQL count on the table "account.fb_access_tokens".
        /// </summary>
        /// <returns>Returns the number of rows of the table "account.fb_access_tokens".</returns>
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
                    Log.Information("Access to count entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT COUNT(*) FROM account.fb_access_tokens;";
            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "account.fb_access_tokens" to return all instances of the "FbAccessToken" class. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "FbAccessToken" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetAll()
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
                    Log.Information("Access to the export entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "account.fb_access_tokens" to return all instances of the "FbAccessToken" class to export. 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instances of "FbAccessToken" class.</returns>
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
                    Log.Information("Access to the export entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id;";
            return Factory.Get<dynamic>(this._Catalog, sql);
        }

        /// <summary>
        /// Executes a select query on the table "account.fb_access_tokens" with a where filter on the column "user_id" to return a single instance of the "FbAccessToken" class. 
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped instance of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Account.Entities.FbAccessToken Get(int userId)
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
                    Log.Information("Access to the get entity \"FbAccessToken\" filtered by \"UserId\" with value {UserId} was denied to the user with Login ID {_LoginId}", userId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens WHERE user_id=@0;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql, userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first record of the table "account.fb_access_tokens". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Account.Entities.FbAccessToken GetFirst()
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
                    Log.Information("Access to the get the first record of entity \"FbAccessToken\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id LIMIT 1;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Gets the previous record of the table "account.fb_access_tokens" sorted by userId.
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Account.Entities.FbAccessToken GetPrevious(int userId)
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
                    Log.Information("Access to the get the previous entity of \"FbAccessToken\" by \"UserId\" with value {UserId} was denied to the user with Login ID {_LoginId}", userId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens WHERE user_id < @0 ORDER BY user_id DESC LIMIT 1;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql, userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets the next record of the table "account.fb_access_tokens" sorted by userId.
        /// </summary>
        /// <param name="userId">The column "user_id" parameter used to find the next record.</param>
        /// <returns>Returns a non-live, non-mapped instance of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Account.Entities.FbAccessToken GetNext(int userId)
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
                    Log.Information("Access to the get the next entity of \"FbAccessToken\" by \"UserId\" with value {UserId} was denied to the user with Login ID {_LoginId}", userId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens WHERE user_id > @0 ORDER BY user_id LIMIT 1;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql, userId).FirstOrDefault();
        }


        /// <summary>
        /// Gets the last record of the table "account.fb_access_tokens". 
        /// </summary>
        /// <returns>Returns a non-live, non-mapped instance of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public Frapid.Account.Entities.FbAccessToken GetLast()
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
                    Log.Information("Access to the get the last record of entity \"FbAccessToken\" was denied to the user with Login ID {_LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id DESC LIMIT 1;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql).FirstOrDefault();
        }

        /// <summary>
        /// Executes a select query on the table "account.fb_access_tokens" with a where filter on the column "user_id" to return a multiple instances of the "FbAccessToken" class. 
        /// </summary>
        /// <param name="userIds">Array of column "user_id" parameter used on where filter.</param>
        /// <returns>Returns a non-live, non-mapped collection of "FbAccessToken" class mapped to the database row.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> Get(int[] userIds)
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
                    Log.Information("Access to entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. userIds: {userIds}.", this._LoginId, userIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens WHERE user_id IN (@0);";

            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql, userIds);
        }

        /// <summary>
        /// Custom fields are user defined form elements for account.fb_access_tokens.
        /// </summary>
        /// <returns>Returns an enumerable custom field collection for the table account.fb_access_tokens</returns>
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
                    Log.Information("Access to get custom fields for entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql;
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                sql = "SELECT * FROM config.custom_field_definition_view WHERE table_name='account.fb_access_tokens' ORDER BY field_order;";
                return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql);
            }

            sql = "SELECT * from config.get_custom_field_definition('account.fb_access_tokens'::text, @0::text) ORDER BY field_order;";
            return Factory.Get<Frapid.DataAccess.Models.CustomField>(this._Catalog, sql, resourceId);
        }

        /// <summary>
        /// Displayfields provide a minimal name/value context for data binding the row collection of account.fb_access_tokens.
        /// </summary>
        /// <returns>Returns an enumerable name and value collection for the table account.fb_access_tokens</returns>
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
                    Log.Information("Access to get display field for entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT user_id AS key, user_id as value FROM account.fb_access_tokens;";
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
        /// Inserts or updates the instance of FbAccessToken class on the database table "account.fb_access_tokens".
        /// </summary>
        /// <param name="fbAccessToken">The instance of "FbAccessToken" class to insert or update.</param>
        /// <param name="customFields">The custom field collection.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object AddOrEdit(dynamic fbAccessToken, List<Frapid.DataAccess.Models.CustomField> customFields)
        {
            if (string.IsNullOrWhiteSpace(this._Catalog))
            {
                return null;
            }



            object primaryKeyValue = fbAccessToken.user_id;

            if (Cast.To<int>(primaryKeyValue) > 0)
            {
                this.Update(fbAccessToken, Cast.To<int>(primaryKeyValue));
            }
            else
            {
                primaryKeyValue = this.Add(fbAccessToken);
            }

            string sql = "DELETE FROM config.custom_fields WHERE custom_field_setup_id IN(" +
                         "SELECT custom_field_setup_id " +
                         "FROM config.custom_field_setup " +
                         "WHERE form_name=config.get_custom_field_form_name('account.fb_access_tokens')" +
                         ");";

            Factory.NonQuery(this._Catalog, sql);

            if (customFields == null)
            {
                return primaryKeyValue;
            }

            foreach (var field in customFields)
            {
                sql = "INSERT INTO config.custom_fields(custom_field_setup_id, resource_id, value) " +
                      "SELECT config.get_custom_field_setup_id_by_table_name('account.fb_access_tokens', @0::character varying(100)), " +
                      "@1, @2;";

                Factory.NonQuery(this._Catalog, sql, field.FieldName, primaryKeyValue, field.Value);
            }

            return primaryKeyValue;
        }

        /// <summary>
        /// Inserts the instance of FbAccessToken class on the database table "account.fb_access_tokens".
        /// </summary>
        /// <param name="fbAccessToken">The instance of "FbAccessToken" class to insert.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public object Add(dynamic fbAccessToken)
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
                    Log.Information("Access to add entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. {FbAccessToken}", this._LoginId, fbAccessToken);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            return Factory.Insert(this._Catalog, fbAccessToken, "account.fb_access_tokens", "user_id");
        }

        /// <summary>
        /// Inserts or updates multiple instances of FbAccessToken class on the database table "account.fb_access_tokens";
        /// </summary>
        /// <param name="fbAccessTokens">List of "FbAccessToken" class to import.</param>
        /// <returns></returns>
        public List<object> BulkImport(List<ExpandoObject> fbAccessTokens)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ImportData, this._LoginId, this._Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to import entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. {fbAccessTokens}", this._LoginId, fbAccessTokens);
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
                        foreach (dynamic fbAccessToken in fbAccessTokens)
                        {
                            line++;



                            object primaryKeyValue = fbAccessToken.user_id;

                            if (Cast.To<int>(primaryKeyValue) > 0)
                            {
                                result.Add(fbAccessToken.user_id);
                                db.Update("account.fb_access_tokens", "user_id", fbAccessToken, fbAccessToken.user_id);
                            }
                            else
                            {
                                result.Add(db.Insert("account.fb_access_tokens", "user_id", fbAccessToken));
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
        /// Updates the row of the table "account.fb_access_tokens" with an instance of "FbAccessToken" class against the primary key value.
        /// </summary>
        /// <param name="fbAccessToken">The instance of "FbAccessToken" class to update.</param>
        /// <param name="userId">The value of the column "user_id" which will be updated.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Update(dynamic fbAccessToken, int userId)
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
                    Log.Information("Access to edit entity \"FbAccessToken\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}. {FbAccessToken}", userId, this._LoginId, fbAccessToken);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Factory.Update(this._Catalog, fbAccessToken, userId, "account.fb_access_tokens", "user_id");
        }

        /// <summary>
        /// Deletes the row of the table "account.fb_access_tokens" against the primary key value.
        /// </summary>
        /// <param name="userId">The value of the column "user_id" which will be deleted.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Delete(int userId)
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
                    Log.Information("Access to delete entity \"FbAccessToken\" with Primary Key {PrimaryKey} was denied to the user with Login ID {LoginId}.", userId, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "DELETE FROM account.fb_access_tokens WHERE user_id=@0;";
            Factory.NonQuery(this._Catalog, sql, userId);
        }

        /// <summary>
        /// Performs a select statement on table "account.fb_access_tokens" producing a paginated result of 50.
        /// </summary>
        /// <returns>Returns the first page of collection of "FbAccessToken" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult()
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
                    Log.Information("Access to the first page of the entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}.", this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id LIMIT 50 OFFSET 0;";
            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a select statement on table "account.fb_access_tokens" producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result.</param>
        /// <returns>Returns collection of "FbAccessToken" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetPaginatedResult(long pageNumber)
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
                    Log.Information("Access to Page #{Page} of the entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}.", pageNumber, this._LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;
            const string sql = "SELECT * FROM account.fb_access_tokens ORDER BY user_id LIMIT 50 OFFSET @0;";

            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql, offset);
        }

        public List<Frapid.DataAccess.Models.Filter> GetFilters(string catalog, string filterName)
        {
            const string sql = "SELECT * FROM config.filters WHERE object_name='account.fb_access_tokens' AND lower(filter_name)=lower(@0);";
            return Factory.Get<Frapid.DataAccess.Models.Filter>(catalog, sql, filterName).ToList();
        }

        /// <summary>
        /// Performs a filtered count on table "account.fb_access_tokens".
        /// </summary>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns number of rows of "FbAccessToken" class using the filter.</returns>
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
                    Log.Information("Access to count entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM account.fb_access_tokens WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Account.Entities.FbAccessToken(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "account.fb_access_tokens" producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filters">The list of filter conditions.</param>
        /// <returns>Returns collection of "FbAccessToken" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetWhere(long pageNumber, List<Frapid.DataAccess.Models.Filter> filters)
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
                    Log.Information("Access to Page #{Page} of the filtered entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", pageNumber, this._LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;
            Sql sql = Sql.Builder.Append("SELECT * FROM account.fb_access_tokens WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Account.Entities.FbAccessToken(), filters);

            sql.OrderBy("user_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered count on table "account.fb_access_tokens".
        /// </summary>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns number of rows of "FbAccessToken" class using the filter.</returns>
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
                    Log.Information("Access to count entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);
            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM account.fb_access_tokens WHERE 1 = 1");
            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Account.Entities.FbAccessToken(), filters);

            return Factory.Scalar<long>(this._Catalog, sql);
        }

        /// <summary>
        /// Performs a filtered select statement on table "account.fb_access_tokens" producing a paginated result of 50.
        /// </summary>
        /// <param name="pageNumber">Enter the page number to produce the paginated result. If you provide a negative number, the result will not be paginated.</param>
        /// <param name="filterName">The named filter.</param>
        /// <returns>Returns collection of "FbAccessToken" class.</returns>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public IEnumerable<Frapid.Account.Entities.FbAccessToken> GetFiltered(long pageNumber, string filterName)
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
                    Log.Information("Access to Page #{Page} of the filtered entity \"FbAccessToken\" was denied to the user with Login ID {LoginId}. Filter: {Filter}.", pageNumber, this._LoginId, filterName);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            List<Frapid.DataAccess.Models.Filter> filters = this.GetFilters(this._Catalog, filterName);

            long offset = (pageNumber - 1) * 50;
            Sql sql = Sql.Builder.Append("SELECT * FROM account.fb_access_tokens WHERE 1 = 1");

            Frapid.DataAccess.FilterManager.AddFilters(ref sql, new Frapid.Account.Entities.FbAccessToken(), filters);

            sql.OrderBy("user_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<Frapid.Account.Entities.FbAccessToken>(this._Catalog, sql);
        }


    }
}