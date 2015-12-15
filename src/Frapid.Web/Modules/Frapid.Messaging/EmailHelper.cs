using System;
using Frapid.Messaging.DTO;
using Frapid.Messaging.Helpers;

namespace Frapid.Messaging
{
    public static class EmailHelper
    {
        public static EmailMessage GetMessage(EmailQueue mail)
        {
            return new EmailMessage
            {
                FromName = mail.FromName,
                FromEmail = mail.ReplyTo,
                Subject = mail.Subject,
                SentTo = mail.SendTo,
                Message = mail.Message,
                Type = Type.Outward,
                EventDateUtc = DateTime.UtcNow,
                Status = Status.Unknown
            };
        }

        public static SmtpHost GetSmtpHost(Config config)
        {
            return new SmtpHost
            {
                Address = config.SmtpHost,
                Port = config.SmtpPort,
                EnableSsl = config.EnableSsl,
                DeliveryMethod = config.DeliveryMethod,
                PickupDirectory = config.PickupDirectory
            };
        }

        public static ICredentials GetCredentials(Config config)
        {
            return new SmtpCredentials
            {
                Username = config.SmtpUsername,
                Password = config.SmtpUserPassword
            };

        }

    }
}