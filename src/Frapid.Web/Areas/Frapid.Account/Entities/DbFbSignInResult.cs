// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.fb_sign_in")]
    [PrimaryKey("", AutoIncrement = false)]
    public sealed class DbFbSignInResult : IPoco
    {
        public long LoginId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}