// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Config.Entities
{
    [TableName("config.smtp_configs")]
    [PrimaryKey("smtp_id", AutoIncrement = true)]
    public sealed class SmtpConfig : IPoco
    {
        public int SmtpId { get; set; }
        public string ConfigurationName { get; set; }
        public bool Enabled { get; set; }
        public bool IsDefault { get; set; }
        public string FromDisplayName { get; set; }
        public string FromEmailAddress { get; set; }
        public string SmtpHost { get; set; }
        public bool SmtpEnableSsl { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public int SmtpPort { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}