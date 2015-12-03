using System.Security;

namespace Frapid.Messaging
{
    public interface ICredentials
    {
        string Username { get; set; }
        SecureString Password { get; set; }
    }
}