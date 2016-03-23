using System;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.Smtp
{
    public static class EmailHelper
    {
        public static EmailMessage GetMessage(Config config, EmailQueue mail)
        {
            var message = new EmailMessage
            {
                FromName = mail.FromName,
                FromEmail = mail.FromEmail,
                ReplyToEmail = mail.ReplyTo,
                ReplyToName = mail.ReplyToName,
                Subject = mail.Subject,
                SentTo = mail.SendTo,
                Message = mail.Message,
                Type = Type.Outward,
                EventDateUtc = DateTime.UtcNow,
                Status = Status.Unknown
            };


            return message;
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