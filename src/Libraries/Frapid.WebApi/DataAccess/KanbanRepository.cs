using System.Collections.Generic;
using System.Linq;
using Frapid.DataAccess;
using Frapid.DataAccess.Models;
using Frapid.DbPolicy;
using Serilog;

namespace Frapid.WebApi.DataAccess
{
    public class KanbanRepository : DbAccess
    {
        public KanbanRepository(string catalog, long loginId, int userId)
        {
            this._ObjectNamespace = "config";
            this._ObjectName = "kanban_details";
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

        public IEnumerable<dynamic> Get(long[] kanbanIds, object[] resourceIds)
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
                        "Access to entity \"KanbanDetail\" was denied to the user with Login ID {LoginId}. KanbanId: {KanbanIds}, ResourceIds {ResourceIds}.",
                        this.LoginId, kanbanIds, resourceIds);
                    throw new UnauthorizedException("Access is denied.");
                }
            }


            if (kanbanIds == null || resourceIds == null || !kanbanIds.Any() || !resourceIds.Any())
            {
                return new List<dynamic>();
            }


            const string sql = "SELECT * FROM config.kanban_details WHERE kanban_id IN(@kanbans) AND resource_id IN (@resources);";
            return Factory.Get<dynamic>(this.Catalog, sql,
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