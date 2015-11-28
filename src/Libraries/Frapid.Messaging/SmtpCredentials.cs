using System.Security;

namespace Frapid.Messaging
{
    public sealed class SmtpCredentials : ICredentials
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}