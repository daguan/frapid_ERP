// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.refresh_tokens")]
    [PrimaryKey("refresh_token_id", AutoIncrement = false)]
    public sealed class RefreshToken : IPoco
    {
        public System.Guid RefreshTokenId { get; set; }
        public System.Guid ApplicationId { get; set; }
        public long LoginId { get; set; }
        public int UserId { get; set; }
        public int OfficeId { get; set; }
        public string TokenInfo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool Revoked { get; set; }
        public int? RevokedBy { get; set; }
        public DateTime RevokedOn { get; set; }
    }
}