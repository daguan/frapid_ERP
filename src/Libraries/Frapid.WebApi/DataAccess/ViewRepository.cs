using System.Collections.Generic;
using System.Linq;
using Frapid.ApplicationState.Cache;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class ViewRepository : DbAccess, IViewRepository
    {
        public ViewRepository(string schemaName, string tableName, string catalog, long loginId, int userId)
        {
            this._ObjectNamespace = Sanitizer.SanitizeIdentifierName(schemaName);
            this._ObjectName = Sanitizer.SanitizeIdentifierName(tableName.Replace("-", "_"));
            this.LoginId = AppUsers.GetCurrent().LoginId;
            this.OfficeId = AppUsers.GetCurrent().OfficeId;
            this.UserId = AppUsers.GetCurrent().UserId;
            this.Catalog = catalog;
            this.LoginId = loginId;
            this.UserId = userId;

            if (!string.IsNullOrWhiteSpace(this._ObjectNamespace) && !string.IsNullOrWhiteSpace(this._ObjectName))
            {
                this.FullyQualifiedObjectName = this._ObjectNamespace + "." + this._ObjectName;
                this.PrimaryKey = this.GetCandidateKeyByConvention();
                this.NameColumn = this.GetNameColumnByConvention();
                this.IsValid = true;
            }
        }

        public sealed override string _ObjectNamespace { get; }
        public sealed override string _ObjectName { get; }
        public string FullyQualifiedObjectName { get; set; }
        public string PrimaryKey { get; set; }
        public string NameColumn { get; set; }
        public string Catalog { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }

        public long Count()
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName};";
            return Factory.Scalar<long>(this.Catalog, sql);
        }

        public IEnumerable<dynamic> Get()
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ExportData, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the export entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey}";
            return Factory.Get<dynamic>(this.Catalog, sql);
        }

        public IEnumerable<DisplayField> GetDisplayFields()
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return new List<DisplayField>();
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
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
            return Factory.Get<DisplayField>(this.Catalog, sql);
        }

        public IEnumerable<dynamic> GetPaginatedResult()
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to the first page of the entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey} LIMIT 50 OFFSET 0;";
            return Factory.Get<dynamic>(this.Catalog, sql);
        }

        public IEnumerable<dynamic> GetPaginatedResult(long pageNumber)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
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

            return Factory.Get<dynamic>(this.Catalog, sql, offset);
        }

        public List<Filter> GetFilters(string catalog, string filterName)
        {
            string sql =
                $"SELECT * FROM config.filters WHERE object_name='{this.FullyQualifiedObjectName}' AND lower(filter_name)=lower(@0);";
            return Factory.Get<Filter>(catalog, sql, filterName).ToList();
        }

        public long CountWhere(List<Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
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

            return Factory.Scalar<long>(this.Catalog, sql);
        }

        public IEnumerable<dynamic> GetWhere(long pageNumber, List<Filter> filters)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
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

            sql.OrderBy("1");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<dynamic>(this.Catalog, sql);
        }

        public long CountFiltered(string filterName)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return 0;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = this.GetFilters(this.Catalog, filterName);
            var sql = Sql.Builder.Append($"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");
            FilterManager.AddFilters(ref sql, filters);

            return Factory.Scalar<long>(this.Catalog, sql);
        }

        public IEnumerable<dynamic> GetFiltered(long pageNumber, string filterName)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return null;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information(
                        $"Access to Page #{pageNumber} of the filtered entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = this.GetFilters(this.Catalog, filterName);

            long offset = (pageNumber - 1)*50;
            var sql = Sql.Builder.Append($"SELECT * FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, filters);

            sql.OrderBy("1");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<dynamic>(this.Catalog, sql);
        }

        #region View to Table Convention

        private string GetTableByConvention()
        {
            string tableName = this._ObjectName;

            tableName = tableName.Replace("_scrud_view", "");
            tableName = tableName.Replace("_view", "");

            return tableName;
        }

        private string GetCandidateKeyByConvention()
        {
            string candidateKey = Inflector.MakeSingular(this.GetTableByConvention());

            if (!string.IsNullOrWhiteSpace(candidateKey))
            {
                candidateKey += "_id";
            }

            return candidateKey ?? "";
        }

        private string GetNameColumnByConvention()
        {
            string nameKey = Inflector.MakeSingular(this.GetTableByConvention());

            if (!string.IsNullOrWhiteSpace(nameKey))
            {
                nameKey += "_name";
            }

            return nameKey ?? "";
        }

        #endregion
    }
}