using System.Threading.Tasks;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging
{
    public interface IEmailProcessor
    {
        Task<bool> SendAsync(EmailMessage email, SmtpHost host, ICredentials credentials,
            bool deleteAttachmentes, params string[] attachments);
    }
}