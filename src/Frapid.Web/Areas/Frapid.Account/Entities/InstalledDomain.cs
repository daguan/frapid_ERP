// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.installed_domains")]
    [PrimaryKey("domain_id", AutoIncrement = true)]
    public sealed class InstalledDomain : IPoco
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public string AdminEmail { get; set; }
    }
}