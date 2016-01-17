// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.access_tokens")]
    [PrimaryKey("access_token_id", AutoIncrement = false)]
    public sealed class AccessToken : IPoco
    {
        public System.Guid AccessTokenId { get; set; }
        public string IssuedBy { get; set; }
        public string Audience { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Header { get; set; }
        public string Subject { get; set; }
        public string TokenId { get; set; }
        public System.Guid? ApplicationId { get; set; }
        public long LoginId { get; set; }
        public string ClientToken { get; set; }
        public string Claims { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool Revoked { get; set; }
        public int? RevokedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
    }
}