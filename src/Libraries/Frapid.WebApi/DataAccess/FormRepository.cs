using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class FormRepository : DbAccess, IFormRepository
    {
        public FormRepository(string schemaName, string tableName, string database, long loginId, int userId)
        {
            this._ObjectNamespace = Sanitizer.SanitizeIdentifierName(schemaName);
            this._ObjectName = Sanitizer.SanitizeIdentifierName(tableName.Replace("-", "_"));
            this.LoginId = AppUsers.GetCurrent().LoginId;
            this.OfficeId = AppUsers.GetCurrent().OfficeId;
            this.UserId = AppUsers.GetCurrent().UserId;
            this.Database = database;
            this.LoginId = loginId;
            this.UserId = userId;

            if (!string.IsNullOrWhiteSpace(this._ObjectNamespace) && !string.IsNullOrWhiteSpace(this._ObjectName))
            {
                this.FullyQualifiedObjectName = this._ObjectNamespace + "." + this._ObjectName;
                this.PrimaryKey = this.GetCandidateKey();
                this.NameColumn = this.GetNameColumn();
                this.IsValid = true;
            }
        }

        public sealed override string _ObjectNamespace { get; }
        public sealed override string _ObjectName { get; }
        public string FullyQualifiedObjectName { get; set; }
        public string PrimaryKey { get; set; }
        public string NameColumn { get; set; }
        public string Database { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }


        public long Count()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName};";
            return Factory.Scalar<long>(this.Database, sql);
        }

        public IEnumerable<dynamic> GetAll()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ExportData, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the export entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName}";
            if (!string.IsNullOrWhiteSpace(this.PrimaryKey))
            {
                sql += $" ORDER BY {this.PrimaryKey}";
            }

            return Factory.Get<dynamic>(this.Database, sql);
        }

        public dynamic Get(object primaryKey)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the get entity \"{this.FullyQualifiedObjectName}\" filtered by \"{this.PrimaryKey}\" with value {primaryKey} was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            if (string.IsNullOrWhiteSpace(this.PrimaryKey))
            {
                return null;
            }


            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} WHERE {this.PrimaryKey}=@0;";
            return Factory.Get<dynamic>(this.Database, sql, primaryKey).FirstOrDefault();
        }

        public dynamic GetFirst()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the get the first record of entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey} LIMIT 1;";
            return Factory.Get<dynamic>(this.Database, sql).FirstOrDefault();
        }

        public dynamic GetPrevious(object primaryKey)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the get the previous entity of \"{this.FullyQualifiedObjectName}\" by \"{this.PrimaryKey}\" with value {primaryKey} was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql =
                $"SELECT * FROM {this.FullyQualifiedObjectName} WHERE {this.PrimaryKey} < @0 ORDER BY {this.PrimaryKey} DESC LIMIT 1;";
            return Factory.Get<dynamic>(this.Database, sql, primaryKey).FirstOrDefault();
        }

        public dynamic GetNext(object primaryKey)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the get the next entity of \"{this.FullyQualifiedObjectName}\" by \"{this.PrimaryKey}\" with value {primaryKey} was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql =
                $"SELECT * FROM {this.FullyQualifiedObjectName} WHERE {this.PrimaryKey} > @0 ORDER BY {this.PrimaryKey} LIMIT 1;";
            return Factory.Get<dynamic>(this.Database, sql, primaryKey).FirstOrDefault();
        }


        public dynamic GetLast()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the get the last record of entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey} DESC LIMIT 1;";
            return Factory.Get<dynamic>(this.Database, sql).FirstOrDefault();
        }

        public IEnumerable<dynamic> Get(object[] primaryKeys)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Keys: {primaryKeys}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} WHERE {this.PrimaryKey} IN (@primaryKeys);";

            return Factory.Get<dynamic>(this.Database, sql, new {primaryKeys});
        }

        public IEnumerable<CustomField> GetCustomFields(string resourceId)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to get custom fields for entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql;
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                sql =
                    $"SELECT * FROM config.custom_field_definition_view WHERE table_name='{this.FullyQualifiedObjectName}' ORDER BY field_order;";
                return Factory.Get<CustomField>(this.Database, sql);
            }

            sql =
                $"SELECT * from config.get_custom_field_definition('{this.FullyQualifiedObjectName}'::text, @0::text) ORDER BY field_order;";
            return Factory.Get<CustomField>(this.Database, sql, resourceId);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return new List<DisplayField>();
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to get display field for entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}",
                        this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql =
                $"SELECT {this.PrimaryKey} AS key, {this.NameColumn} as value FROM {this.FullyQualifiedObjectName};";
            return Factory.Get<DisplayField>(this.Database, sql);
        }

        public object AddOrEdit(dynamic item, List<CustomField> customFields)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            item.audit_user_id = this.UserId;
            item.audit_ts = DateTime.UtcNow;

            object primaryKeyValue = PropertyManager.GetPropertyValue(item, this.PrimaryKey);

            if (primaryKeyValue != null)
            {
                this.Update(item, primaryKeyValue);
            }
            else
            {
                primaryKeyValue = this.Add(item);
            }

            string sql = $"DELETE FROM config.custom_fields WHERE custom_field_setup_id IN(" +
                         "SELECT custom_field_setup_id " +
                         "FROM config.custom_field_setup " +
                         "WHERE form_name=config.get_custom_field_form_name('{this.FullyQualifiedObjectName}')" +
                         ");";

            Factory.NonQuery(this.Database, sql);

            if (customFields == null)
            {
                return primaryKeyValue;
            }

            foreach (var field in customFields)
            {
                sql = $"INSERT INTO config.custom_fields(custom_field_setup_id, resource_id, value) " +
                      "SELECT config.get_custom_field_setup_id_by_table_name('{this.FullyQualifiedObjectName}', @0::character varying(100)), " +
                      "@1, @2;";

                Factory.NonQuery(this.Database, sql, field.FieldName, primaryKeyValue, field.Value);
            }

            return primaryKeyValue;
        }

        public object Add(dynamic item)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Create, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to add entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. {item}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            return Factory.Insert(this.Database, item, this.FullyQualifiedObjectName, this.PrimaryKey);
        }

        public List<object> BulkImport(List<ExpandoObject> items)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ImportData, this.LoginId, this.Database, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to import entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var result = new List<object>();
            int line = 0;
            try
            {
                using (var db = new Database(ConnectionString.GetConnectionString(this.Database), Factory.ProviderName))
                {
                    using (var transaction = db.GetTransaction())
                    {
                        foreach (dynamic item in items)
                        {
                            line++;

                            item.audit_user_id = this.UserId;
                            item.audit_ts = DateTime.UtcNow;

                            object primaryKeyValue = PropertyManager.GetPropertyValue(item, this.PrimaryKey);

                            if (primaryKeyValue != null)
                            {
                                result.Add(primaryKeyValue);
                                db.Update(this.FullyQualifiedObjectName, this.PrimaryKey, item, primaryKeyValue);
                            }
                            else
                            {
                                result.Add(db.Insert(this.FullyQualifiedObjectName, this.PrimaryKey, item));
                            }
                        }

                        transaction.Complete();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error on line {line}. {ex.Message} ";
                throw new DataAccessException(errorMessage, ex);
            }
        }

        public void Update(dynamic item, object primaryKey)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Edit, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to edit entity \"{this.FullyQualifiedObjectName}\" with Primary Key {this.PrimaryKey} was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            Factory.Update(this.Database, item, primaryKey, this.FullyQualifiedObjectName, this.PrimaryKey);
        }

        public void Delete(object primaryKey)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Delete, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to delete entity \"{this.FullyQualifiedObjectName}\" with Primary Key {this.PrimaryKey} was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"DELETE FROM {this.FullyQualifiedObjectName} WHERE {this.PrimaryKey}=@0;";
            Factory.NonQuery(this.Database, sql, primaryKey);
        }

        public IEnumerable<dynamic> GetPaginatedResult()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the first page of the entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey} LIMIT 50 OFFSET 0;";
            return Factory.Get<dynamic>(this.Database, sql);
        }

        public IEnumerable<dynamic> GetPaginatedResult(long pageNumber)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to Page #{pageNumber} of the entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1)*50;
            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey} LIMIT 50 OFFSET @0;";

            return Factory.Get<dynamic>(this.Database, sql, offset);
        }

        public List<Filter> GetFilters(string database, string filterName)
        {
            string sql =
                $"SELECT * FROM config.filters WHERE object_name='{this.FullyQualifiedObjectName}' AND lower(filter_name)=lower(@0);";
            return Factory.Get<Filter>(database, sql, filterName).ToList();
        }

        public long CountWhere(List<Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filters: {filters}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var sql = Sql.Builder.Append($"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");
            FilterManager.AddFilters(ref sql, filters);

            return Factory.Scalar<long>(this.Database, sql);
        }

        public IEnumerable<dynamic> GetWhere(long pageNumber, List<Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to Page #{pageNumber} of the filtered entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filters: {filters}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1)*50;
            var sql = Sql.Builder.Append($"SELECT * FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, filters);

            if (!string.IsNullOrWhiteSpace(this.PrimaryKey))
            {
                sql.OrderBy(this.PrimaryKey);
            }

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<dynamic>(this.Database, sql);
        }

        public long CountFiltered(string filterName)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = this.GetFilters(this.Database, filterName);
            var sql = Sql.Builder.Append($"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");
            FilterManager.AddFilters(ref sql, filters);

            return Factory.Scalar<long>(this.Database, sql);
        }

        public IEnumerable<dynamic> GetFiltered(long pageNumber, string filterName)
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to Page #{pageNumber} of the filtered entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = this.GetFilters(this.Database, filterName);

            long offset = (pageNumber - 1)*50;
            var sql = Sql.Builder.Append($"SELECT * FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, filters);

            if (!string.IsNullOrWhiteSpace(this.PrimaryKey))
            {
                sql.OrderBy(this.PrimaryKey);
            }

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<dynamic>(this.Database, sql);
        }

        private string GetTableName()
        {
            string tableName = this._ObjectName.Replace("-", "_");
            return tableName;
        }

        private string GetCandidateKey()
        {
            string candidateKey = Inflector.MakeSingular(this._ObjectName);
            if (!string.IsNullOrWhiteSpace(candidateKey))
            {
                candidateKey += "_id";
            }

            return candidateKey ?? "";
        }

        private string GetNameColumn()
        {
            string nameKey = Inflector.MakeSingular(this._ObjectName);

            if (!string.IsNullOrWhiteSpace(nameKey))
            {
                nameKey += "_name";
            }

            return nameKey ?? "";
        }

        public EntityView GetMeta()
        {
            if (string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to view meta information on entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            return EntityView.Get(this.Database, this.PrimaryKey, this._ObjectNamespace, this.GetTableName());
        }
    }
}