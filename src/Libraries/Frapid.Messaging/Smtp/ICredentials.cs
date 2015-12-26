using System.Security;

namespace Frapid.Messaging.Smtp
{
    public interface ICredentials
    {
        string Username { get; set; }
        SecureString Password { get; set; }
    }
}