using System.Collections.Generic;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class FlagRepository : DbAccess
    {
        public FlagRepository(string catalog, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "flag_view";
            this.Catalog = catalog;
            this.LoginId = loginId;
            this.UserId = userId;
        }

        public override string _ObjectNamespace { get; }
        public override string _ObjectName { get; }
        public string NameColumn { get; set; }
        public string Catalog { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public long LoginId { get; set; }
        public int OfficeId { get; set; }

        public IEnumerable<dynamic> Get(string resource, int userId, object[] resourceIds)
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
                    Log.Information("Access to entity \"FlagView\" was denied to the user with Login ID {LoginId}. Resource: {Resource}, ResourceIds {ResourceIds}.", this.LoginId, resource, resourceIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.flag_view WHERE resource=@0 AND user_id=@1 AND resource_id IN (@2);";

            return Factory.Get<dynamic>(this.Catalog, sql, resource, userId, resourceIds);
        }
    }
}