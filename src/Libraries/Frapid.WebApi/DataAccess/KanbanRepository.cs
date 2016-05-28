using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class KanbanRepository: DbAccess
    {
        public KanbanRepository(string database, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "kanban_details";
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

        public async Task<IEnumerable<dynamic>> GetAsync(long[] kanbanIds, object[] resourceIds)
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
                    Log.Information("Access to entity \"KanbanDetail\" was denied to the user with Login ID {LoginId}. KanbanId: {KanbanIds}, ResourceIds {ResourceIds}.", this.LoginId, kanbanIds, resourceIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }


            if(kanbanIds == null ||
               resourceIds == null ||
               !kanbanIds.Any() ||
               !resourceIds.Any())
            {
                return new List<dynamic>();
            }


            const string sql = "SELECT * FROM config.kanban_details WHERE kanban_id IN(@kanbans) AND resource_id IN (@resources);";
            return await Factory.GetAsync<dynamic>
                (
                 this.Database,
                 sql,
                 new
                 {
                     kanbans = kanbanIds
                 },
                 new
                 {
                     resources = resourceIds
                 });
        }
    }
}