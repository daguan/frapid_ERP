using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class FlagRepository: DbAccess
    {
        public FlagRepository(string database, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "flag_view";
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

        public async Task<IEnumerable<dynamic>> GetAsync(string resource, int userId, object[] resourceIds)
        {
            if(string.IsNullOrWhiteSpace(this.Database))
            {
                return null;
            }

            if(!this.SkipValidation)
            {
                if(!this.Validated)
                {
                    await this.ValidateAsync(AccessTypeEnum.Read, this.LoginId, this.Database, false).ConfigureAwait(false);
                }
                if(!this.HasAccess)
                {
                    Log.Information("Access to entity \"FlagView\" was denied to the user with Login ID {LoginId}. Resource: {Resource}, ResourceIds {ResourceIds}.", this.LoginId, resource, resourceIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }

            const string sql = "SELECT * FROM config.flag_view WHERE resource=@0 AND user_id=@1 AND resource_id IN (@2);";

            return await Factory.GetAsync<dynamic>(this.Database, sql, resource, userId, resourceIds).ConfigureAwait(false);
        }
    }
}