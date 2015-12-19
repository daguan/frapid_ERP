// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.logins")]
    [PrimaryKey("login_id", AutoIncrement = true)]
    public sealed class Login : IPoco
    {
        public long LoginId { get; set; }
        public int? UserId { get; set; }
        public int? OfficeId { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public string Culture { get; set; }
    }
}