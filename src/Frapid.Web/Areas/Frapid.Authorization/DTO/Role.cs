using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Authorization.DTO
{
    [TableName("account.roles")]
    public class Role : IPoco
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}