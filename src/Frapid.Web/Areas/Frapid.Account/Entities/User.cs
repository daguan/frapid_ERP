// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.users")]
    [PrimaryKey("user_id", AutoIncrement = true)]
    public sealed class User : IPoco
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int OfficeId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}