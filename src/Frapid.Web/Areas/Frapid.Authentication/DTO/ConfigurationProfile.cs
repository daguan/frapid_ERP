using Frapid.DataAccess;

namespace Frapid.Authentication.DTO
{
    public class ConfigurationProfile:IPoco
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public bool IsActive { get; set; }
        public bool AllowRegistration { get; set; }
        public int RegistrationRoleId { get; set; }
        public bool AllowFacebookRegistration { get; set; }
        public bool AllowGoogleRegistration { get; set; }
        public string GoogleSigninClientId { get; set; }
        public string GoogleSignInScope { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookScope { get; set; }
    }
}