namespace Frapid.Authentication.ViewModels
{
    public class ResetInputModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
    }
}