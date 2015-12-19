// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.reset_account")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class DbResetAccountResult : IPoco
    {
        public System.Guid RequestId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public bool Confirmed { get; set; }
        public DateTime ConfirmedOn { get; set; }
    }
}