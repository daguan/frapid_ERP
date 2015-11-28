using Frapid.DataAccess;

namespace Frapid.Authentication.DTO
{
    public class SignInResult : IPoco
    {
        public long SignInId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}