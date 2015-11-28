namespace Frapid.Authentication.Models
{
    public class GoogleUserInfo : IUserInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}