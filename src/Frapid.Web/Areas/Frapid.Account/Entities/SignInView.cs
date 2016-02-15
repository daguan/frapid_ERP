// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.sign_in_view")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class SignInView : IPoco
    {
        public long? LoginId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsAdministrator { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LoginTimestamp { get; set; }
        public string Culture { get; set; }
        public bool? IsActive { get; set; }
        public int? OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string Office { get; set; }
        public DateTime? LastSeenOn { get; set; }
    }
}