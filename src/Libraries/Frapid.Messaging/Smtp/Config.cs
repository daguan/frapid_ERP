using System.Net.Mail;
using System.Security;
using System.Text;
using System.Web.Security;
using Frapid.Messaging.DTO;

namespace Frapid.Messaging.Smtp
{
    public class Config : IEmailConfig
    {
        public Config(string tenant, IEmailProcessor processor)
        {
            if (processor != null)
            {
                this.Tenant = tenant;
                this.Enabled = processor.IsEnabled;
                this.FromEmail = processor.Config.FromEmail;
                this.FromName = processor.Config.FromName;
                return;
            }


            //We do not have transactional email processor.
            //Fall back to SMTP configuration


            var smtp = GetSmtpConfig(tenant);

            if (smtp == null)
            {
                return;
            }

            this.Tenant = tenant;
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

        public string Tenant { get; set; }
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
            if (string.IsNullOrWhiteSpace(password))
            {
                return new SecureString();
            }

            var data = Encoding.UTF8.GetBytes(password);
            var unsecure = MachineKey.Unprotect(data, "ScrudFactory");

            if (unsecure == null)
            {
                return new SecureString();
            }

            password = Encoding.UTF8.GetString(unsecure);

            var secureString = new SecureString();
            foreach (char c in password)
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }
    }
}