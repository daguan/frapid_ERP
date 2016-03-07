using System.Net.Mail;
using System.Security;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.Smtp
{
    public class Config : IEmailConfig
    {
        public Config(string database)
        {
            var smtp = GetSmtpConfig(database);

            if (smtp == null)
            {
                return;
            }
            this.Database = database;
            this.Enabled = smtp.Enabled;
            this.FromName = smtp.FromDisplayName;
            this.FromEmail = smtp.FromEmailAddress;
            this.SmtpHost = smtp.SmtpHost;
            this.EnableSsl = smtp.SmtpEnableSsl;
            this.SmtpPort = smtp.SmtpPort;
            this.SmtpUsername = smtp.SmtpUsername;
            this.SmtpUserPassword = this.GetSmtpUserPassword(smtp.SmtpPassword);
            this.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public string Database { get; set; }
        public bool Enabled { get; set; }
        public bool EnableSsl { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public SecureString SmtpUserPassword { get; set; }
        public string PickupDirectory { get; set; }

        public static bool IsEnabled(string database)
        {
            var smtp = GetSmtpConfig(database);

            if (smtp == null)
            {
                return false;
            }

            return smtp.Enabled;
        }

        private static SmtpConfig GetSmtpConfig(string database)
        {
            return DAL.Smtp.GetConfig(database);
        }

        private SecureString GetSmtpUserPassword(string password)
        {
            var secureString = new SecureString();
            foreach (char c in password)
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }
    }
}