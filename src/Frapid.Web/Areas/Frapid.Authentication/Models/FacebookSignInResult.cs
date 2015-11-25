namespace Frapid.Authentication.Models
{
    public class FacebookSignInResult
    {
        public long SignInId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}