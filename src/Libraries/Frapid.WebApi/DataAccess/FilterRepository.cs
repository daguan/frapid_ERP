using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Frapid.Configuration;
using Frapid.Configuration.Db;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.NPoco;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class FilterRepository: DbAccess
    {
        public FilterRepository(string database, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "filters";
            this.Database = database;
            this.LoginId = loginId;
            this.UserId = userId;
        }

        public override string _ObjectNamespace { get; }
        public override string _ObjectName { get; }
        public string NameColumn { get; set; }
        public string Database { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }

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
                    Log.Information("Access to Page #{Page} of the filtered entity \"Filter\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", pageNumber, this.LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;
            var sql = Sql.Builder.Append("SELECT * FROM config.filters WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, new Filter(), filters);

            sql.OrderBy("filter_id");

            if(pageNumber > 0)
            {
                sql.Append(FrapidDbServer.AddOffset(this.Database, "@0"), offset);
                sql.Append(FrapidDbServer.AddLimit(this.Database, "@0"), 50);
            }

            return await Factory.GetAsync<dynamic>(this.Database, sql);
        }

        public async Task MakeDefaultAsync(string objectName, string filterName)
        {
            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.CreateFilter, this.LoginId, this.Database, false);
                }

                if(!this.HasAccess)
                {
                    Log.Information("Access to create default filter '{FilterName}' for {ObjectName} was denied to the user with Login ID {LoginId}.", filterName, objectName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "UPDATE config.filters SET is_default=true WHERE object_name=@0 AND filter_name=@1;";
            await Factory.NonQueryAsync(this.Database, sql, objectName, filterName);
        }

        /// <summary>
        ///     Deletes the row of the table "config.filters" against the supplied filter name.
        /// </summary>
        /// <param name="filterName">The value of the column "filter_name" which will be deleted.</param>
        /// <exception cref="UnauthorizedException">
        ///     Thown when the application user does not have sufficient privilege to perform
        ///     this action.
        /// </exception>
        public async Task DeleteAsync(string filterName)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Delete, this.LoginId, this.Database, false);
                }
                if(!this.HasAccess)
                {
                    Log.Information("Access to delete entity \"Filter\" with Filter Name {FilterName} was denied to the user with Login ID {LoginId}.", filterName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "DELETE FROM config.filters WHERE filter_name=@0;";
            await Factory.NonQueryAsync(this.Database, sql, filterName);
        }

        public async Task RecreateFiltersAsync(string objectName, string filterName, List<ExpandoObject> filters)
        {
            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Create, this.LoginId, this.Database, false);
                }

                if(!this.HasAccess)
                {
                    Log.Information("Access to add entity \"Filter\" was denied to the user with Login ID {LoginId}. {filters}", this.LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }


            using(var db = DbProvider.GetDatabase(this.Database))
            {
                using(var transaction = db.GetTransaction())
                {
                    var toDelete = await this.GetWhereAsync
                        (
                         1,
                         new List<Filter>
                         {
                             new Filter
                             {
                                 ColumnName = "object_name",
                                 FilterCondition = (int)FilterCondition.IsEqualTo,
                                 FilterValue = objectName
                             },
                             new Filter
                             {
                                 ColumnName = "filter_name",
                                 FilterCondition = (int)FilterCondition.IsEqualTo,
                                 FilterValue = filterName
                             }
                         });


                    foreach(var filter in toDelete)
                    {
                        await db.DeleteAsync("config.filters", "filter_id", filter);
                    }

                    foreach(dynamic filter in filters)
                    {
                        filter.audit_user_id = this.UserId;
                        filter.audit_ts = DateTimeOffset.UtcNow;

                        await db.InsertAsync("config.filters", "filter_id", true, filter);
                    }

                    transaction.Complete();
                }
            }
        }

        public async Task RemoveDefaultAsync(string objectName)
        {
            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    this.Validate(AccessTypeEnum.CreateFilter, this.LoginId, this.Database, false);
                }

                if(!this.HasAccess)
                {
                    Log.Information("Access to delete default filter for {ObjectName} was denied to the user with Login ID {LoginId}.", objectName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "UPDATE config.filters SET is_default=false WHERE object_name=@0;";
            await Factory.NonQueryAsync(this.Database, sql, objectName);
        }
    }
}