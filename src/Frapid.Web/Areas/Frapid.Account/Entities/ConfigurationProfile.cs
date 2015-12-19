// ReSharper disable All
using System;
using Frapid.DataAccess;
using Frapid.NPoco;

namespace Frapid.Account.Entities
{
    [TableName("account.configuration_profiles")]
    [PrimaryKey("profile_id", AutoIncrement = true)]
    public sealed class ConfigurationProfile : IPoco
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public bool IsActive { get; set; }
        public bool AllowRegistration { get; set; }
        public int RegistrationOfficeId { get; set; }
        public int RegistrationRoleId { get; set; }
        public bool AllowFacebookRegistration { get; set; }
        public bool AllowGoogleRegistration { get; set; }
        public string GoogleSigninClientId { get; set; }
        public string GoogleSigninScope { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookScope { get; set; }
        public int? AuditUserId { get; set; }
        public DateTime? AuditTs { get; set; }
    }
}