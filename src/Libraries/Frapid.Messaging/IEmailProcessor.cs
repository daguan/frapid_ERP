using System.Threading.Tasks;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging
{
    public interface IEmailProcessor
    {
        bool IsEnabled { get; set; }
        void InitializeConfig(string catalog);

        IEmailConfig Config { get; set; }
        Task<bool> SendAsync(EmailMessage email);
        Task<bool> SendAsync(EmailMessage email, bool deleteAttachmentes, params string[] attachments);
    }
}