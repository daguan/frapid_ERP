using System.Net.Mail;
using System.Security;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.Helpers
{
    public class Config
    {
        public Config(string catalog)
        {
            var smtp = GetSmtpConfig(catalog);

            if (smtp == null)
            {
                return;
            }
            this.Catalog = catalog;
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

        public string Catalog { get; set; }
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

        public static bool IsEnabled(string catalog)
        {
            var smtp = GetSmtpConfig(catalog);

            if (smtp == null)
            {
                return false;
            }

            return smtp.Enabled;
        }

        private static Smtp GetSmtpConfig(string catalog)
        {
            return DAL.Smtp.GetConfig(catalog);
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