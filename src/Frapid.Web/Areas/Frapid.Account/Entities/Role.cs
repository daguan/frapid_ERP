// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.roles")]
    [PrimaryKey("role_id", AutoIncrement = false)]
    public sealed class Role : IPoco
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAdministrator { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}