using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.ApplicationState.Cache;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.NPoco;
using Frapid.NPoco.FluentMappings;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class ViewRepository: DbAccess, IViewRepository
    {
        public ViewRepository(string schemaName, string tableName, string database, long loginId, int userId)
        {
            var appUser = AppUsers.GetCurrentAsync().Result;

            this._ObjectNamespace = Sanitizer.SanitizeIdentifierName(schemaName);
            this._ObjectName = Sanitizer.SanitizeIdentifierName(tableName.Replace("-", "_"));
            this.LoginId = appUser.LoginId;
            this.OfficeId = appUser.OfficeId;
            this.UserId = appUser.UserId;
            this.Database = database;
            this.LoginId = loginId;
            this.UserId = userId;

            if(!string.IsNullOrWhiteSpace(this._ObjectNamespace) &&
               !string.IsNullOrWhiteSpace(this._ObjectName))
            {
                this.FullyQualifiedObjectName = this._ObjectNamespace + "." + this._ObjectName;
                this.PrimaryKey = this.GetCandidateKeyByConvention();
                this.LookupField = this.GetLookupFieldByConvention();
                this.NameColumn = this.GetNameColumnByConvention();
                this.IsValid = true;
            }
        }

        public sealed override string _ObjectNamespace { get; }
        public sealed override string _ObjectName { get; }
        public string FullyQualifiedObjectName { get; set; }
        public string PrimaryKey { get; set; }
        public string LookupField { get; set; }
        public string NameColumn { get; set; }
        public string Database { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }

        public async Task<long> CountAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName};";
            return await Factory.ScalarAsync<long>(this.Database, sql);
        }

        public async Task<IEnumerable<dynamic>> GetAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.ExportData, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to the export entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT * FROM {this.FullyQualifiedObjectName} ORDER BY {this.PrimaryKey}";
            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        public async Task<IEnumerable<DisplayField>> GetDisplayFieldsAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return new List<DisplayField>();
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to get display field for entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}", this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT {this.PrimaryKey} AS \"key\", {this.NameColumn} as \"value\" FROM {this.FullyQualifiedObjectName};";
            return await Factory.GetAsync<DisplayField>(this.Database, sql);
        }

        public async Task<IEnumerable<DisplayField>> GetLookupFieldsAsync()
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
                    Log.Information($"Access to get display field for entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}", this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            string sql = $"SELECT {this.LookupField} AS \"key\", {this.NameColumn} as \"value\" FROM {this.FullyQualifiedObjectName};";
            return await Factory.GetAsync<DisplayField>(this.Database, sql);
        }

        public async Task<IEnumerable<dynamic>> GetPaginatedResultAsync()
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to the first page of the entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            //"SELECT * FROM {this.FullyQualifiedObjectName} 
            //ORDER BY {this.PrimaryKey} LIMIT 50 OFFSET 0;";

            var sql = new Sql($"SELECT * FROM {this.FullyQualifiedObjectName}");
            sql.OrderBy(this.PrimaryKey);
            sql.Append(FrapidDbServer.AddOffset(this.Database, "@0"), 0);
            sql.Append(FrapidDbServer.AddLimit(this.Database, "@0"), 50);

            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        public async Task<IEnumerable<dynamic>> GetPaginatedResultAsync(long pageNumber)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to Page #{pageNumber} of the entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;

            //"SELECT * FROM {this.FullyQualifiedObjectName} 
            //ORDER BY {this.PrimaryKey} LIMIT 50 OFFSET @0;";

            var sql = new Sql($"SELECT * FROM {this.FullyQualifiedObjectName}");
            sql.OrderBy(this.PrimaryKey);
            sql.Append(FrapidDbServer.AddOffset(this.Database, "@0"), offset);
            sql.Append(FrapidDbServer.AddLimit(this.Database, "@0"), 50);

            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        public async Task<IEnumerable<Filter>> GetFiltersAsync(string database, string filterName)
        {
            string sql = $"SELECT * FROM config.filters WHERE object_name='{this.FullyQualifiedObjectName}' AND lower(filter_name)=lower(@0);";
            return await Factory.GetAsync<Filter>(database, sql, filterName);
        }

        public async Task<long> CountWhereAsync(List<Filter> filters)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filters: {filters}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var sql = Sql.Builder.Append($"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");
            FilterManager.AddFilters(ref sql, filters);

            return await Factory.ScalarAsync<long>(this.Database, sql);
        }

        public async Task<IEnumerable<dynamic>> GetWhereAsync(long pageNumber, List<Filter> filters)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to Page #{pageNumber} of the filtered entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filters: {filters}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;
            var sql = Sql.Builder.Append($"SELECT * FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, filters);

            sql.OrderBy("1");

            if(pageNumber > 0)
            {
                sql.Append(FrapidDbServer.AddOffset(this.Database, "@0"), offset);
                sql.Append(FrapidDbServer.AddLimit(this.Database, "@0"), 50);
            }

            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        public async Task<long> CountFilteredAsync(string filterName)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return 0;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to count entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = await this.GetFiltersAsync(this.Database, filterName);
            var sql = Sql.Builder.Append($"SELECT COUNT(*) FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");
            FilterManager.AddFilters(ref sql, filters.ToList());

            return await Factory.ScalarAsync<long>(this.Database, sql);
        }

        public async Task<IEnumerable<dynamic>> GetFilteredAsync(long pageNumber, string filterName)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Read, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information($"Access to Page #{pageNumber} of the filtered entity \"{this.FullyQualifiedObjectName}\" was denied to the user with Login ID {this.LoginId}. Filter: {filterName}.");
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            var filters = await this.GetFiltersAsync(this.Database, filterName);

            long offset = (pageNumber - 1) * 50;
            var sql = Sql.Builder.Append($"SELECT * FROM {this.FullyQualifiedObjectName} WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, filters.ToList());

            sql.OrderBy("1");

            if(pageNumber > 0)
            {
                sql.Append(FrapidDbServer.AddOffset(this.Database, "@0"), offset);
                sql.Append(FrapidDbServer.AddLimit(this.Database, "@0"), 50);
            }

            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        #region View to Table Convention

        private string GetTableByConvention()
        {
            string tableName = this._ObjectName;

            tableName = tableName.Replace("_scrud_view", "");
            tableName = tableName.Replace("_selector_view", "");
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

            candidateKey = candidateKey ?? "";

            return Sanitizer.SanitizeIdentifierName(candidateKey);
        }

        private string GetLookupFieldByConvention()
        {
            string candidateKey = Inflector.MakeSingular(this.GetTableByConvention());

            if (!string.IsNullOrWhiteSpace(candidateKey))
            {
                candidateKey += "_code";
            }

            candidateKey = candidateKey ?? "";

            return Sanitizer.SanitizeIdentifierName(candidateKey);
        }

        private string GetNameColumnByConvention()
        {
            string nameKey = Inflector.MakeSingular(this.GetTableByConvention());

            if(!string.IsNullOrWhiteSpace(nameKey))
            {
                nameKey += "_name";
            }

            return nameKey ?? "";
        }

        #endregion
    }
}