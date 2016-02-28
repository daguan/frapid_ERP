using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Frapid.Configuration;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Frapid.NPoco;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class FilterRepository : DbAccess
    {
        public override string _ObjectNamespace { get; }
        public override string _ObjectName { get; }
        public string NameColumn { get; set; }
        public string Catalog { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }

        public FilterRepository(string catalog, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "filters";
            this.Catalog = catalog;
            this.LoginId = loginId;
            this.UserId = userId;
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
                    Log.Information("Access to Page #{Page} of the filtered entity \"Filter\" was denied to the user with Login ID {LoginId}. Filters: {Filters}.", pageNumber, this.LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            long offset = (pageNumber - 1) * 50;
            var sql = Sql.Builder.Append("SELECT * FROM config.filters WHERE 1 = 1");

            FilterManager.AddFilters(ref sql, new Filter(), filters);

            sql.OrderBy("filter_id");

            if (pageNumber > 0)
            {
                sql.Append("LIMIT @0", 50);
                sql.Append("OFFSET @0", offset);
            }

            return Factory.Get<dynamic>(this.Catalog, sql);
        }

        public void MakeDefault(string objectName, string filterName)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.CreateFilter, this.LoginId, this.Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to create default filter '{FilterName}' for {ObjectName} was denied to the user with Login ID {LoginId}.", filterName, objectName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "UPDATE config.filters SET is_default=true WHERE object_name=@0 AND filter_name=@1;";
            Factory.NonQuery(this.Catalog, sql, objectName, filterName);
        }

        /// <summary>
        /// Deletes the row of the table "config.filters" against the supplied filter name.
        /// </summary>
        /// <param name="filterName">The value of the column "filter_name" which will be deleted.</param>
        /// <exception cref="UnauthorizedException">Thown when the application user does not have sufficient privilege to perform this action.</exception>
        public void Delete(string filterName)
        {
            if (string.IsNullOrWhiteSpace(this.Catalog))
            {
                return;
            }

            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Delete, this.LoginId, this.Catalog, false);
                }
                if (!this.HasAccess)
                {
                    Log.Information("Access to delete entity \"Filter\" with Filter Name {FilterName} was denied to the user with Login ID {LoginId}.", filterName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "DELETE FROM config.filters WHERE filter_name=@0;";
            Factory.NonQuery(this.Catalog, sql, filterName);
        }

        public void RecreateFilters(string objectName, string filterName, List<ExpandoObject> filters)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.Create, this.LoginId, this.Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to add entity \"Filter\" was denied to the user with Login ID {LoginId}. {filters}", this.LoginId, filters);
                    throw new UnauthorizedException("Access is denied.");
                }
            }


            using (var db = new Database(ConnectionString.GetConnectionString(this.Catalog), Factory.ProviderName))
            {
                using (var transaction = db.GetTransaction())
                {

                    var toDelete = this.GetWhere(1, new List<Filter>
                    {
                        new Filter { ColumnName = "object_name", FilterCondition = (int) FilterCondition.IsEqualTo, FilterValue = objectName },
                        new Filter { ColumnName = "filter_name", FilterCondition = (int) FilterCondition.IsEqualTo, FilterValue = filterName }
                    });


                    foreach (var filter in toDelete)
                    {
                        db.Delete("config.filters", "filter_id", filter);
                    }

                    foreach (dynamic filter in filters)
                    {
                        filter.audit_user_id = this.UserId;
                        filter.audit_ts = System.DateTime.UtcNow;

                        db.Insert("config.filters", "filter_id", true, filter);
                    }

                    transaction.Complete();
                }
            }
        }

        public void RemoveDefault(string objectName)
        {
            if (!this.SkipValidation)
            {
                if (!this.Validated)
                {
                    this.Validate(AccessTypeEnum.CreateFilter, this.LoginId, this.Catalog, false);
                }

                if (!this.HasAccess)
                {
                    Log.Information("Access to delete default filter for {ObjectName} was denied to the user with Login ID {LoginId}.", objectName, this.LoginId);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "UPDATE config.filters SET is_default=false WHERE object_name=@0;";
            Factory.NonQuery(this.Catalog, sql, objectName);
        }
    }
}