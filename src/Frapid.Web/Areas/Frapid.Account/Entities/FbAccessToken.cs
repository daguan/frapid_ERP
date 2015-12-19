// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.fb_access_tokens")]
    [PrimaryKey("user_id", AutoIncrement = false)]
    public sealed class FbAccessToken : IPoco
    {
        public int UserId { get; set; }
        public string FbUserId { get; set; }
        public string Token { get; set; }
    }
}