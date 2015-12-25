// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.applications")]
    [PrimaryKey("application_id", AutoIncrement = false)]
    public sealed class Application : IPoco
    {
        public System.Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string DisplayName { get; set; }
        public string VersionNumber { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string ApplicationUrl { get; set; }
        public string Description { get; set; }
        public bool BrowserBasedApp { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }
        public string SupportEmail { get; set; }
        public string Culture { get; set; }
        public string RedirectUrl { get; set; }
        public string AppSecret { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}