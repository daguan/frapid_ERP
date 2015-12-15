using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Frapid.Messaging.DTO;
using Serilog;

namespace Frapid.Messaging
{
    public sealed class Processor : IEmailProcessor
    {
        public async Task<bool> SendAsync(EmailMessage email, SmtpHost host, ICredentials credentials,
            bool deleteAttachmentes, params string[] attachments)
        {
            if (string.IsNullOrWhiteSpace(email.SentTo))
            {
                throw new ArgumentNullException(email.SentTo);
            }

            if (string.IsNullOrWhiteSpace(email.Message))
            {
                throw new ArgumentNullException(email.Message);
            }

            string[] addresses = email.SentTo.Split(',');

            foreach (Validator validator in addresses.Select(address => new Validator(address)))
            {
                validator.Validate();

                if (!validator.IsValid)
                {
                    return false;
                }
            }

            addresses = addresses.Distinct().ToArray();
            email.SentTo = string.Join(",", addresses);
            email.Status = Status.Executing;


            using (MailMessage mail = new MailMessage(email.FromEmail, email.SentTo))
            {
                if (attachments != null)
                {
                    foreach (string file in attachments)
                    {
                        if (!string.IsNullOrWhiteSpace(file))
                        {
                            if (File.Exists(file))
                            {
                                Attachment attachment = new Attachment(file, MediaTypeNames.Application.Octet);

                                ContentDisposition disposition = attachment.ContentDisposition;
                                disposition.CreationDate = File.GetCreationTime(file);
                                disposition.ModificationDate = File.GetLastWriteTime(file);
                                disposition.ReadDate = File.GetLastAccessTime(file);

                                disposition.FileName = Path.GetFileName(file);
                                disposition.Size = new FileInfo(file).Length;
                                disposition.DispositionType = DispositionTypeNames.Attachment;

                                mail.Attachments.Add(attachment);
                            }
                        }
                    }
                }


                MailAddress sender = new MailAddress(email.FromEmail, email.FromName);
                using (SmtpClient smtp = new SmtpClient(host.Address, host.Port))
                {
                    smtp.DeliveryMethod = host.DeliveryMethod;
                    smtp.PickupDirectoryLocation = host.PickupDirectory;

                    smtp.EnableSsl = host.EnableSsl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(credentials.Username, credentials.Password);
                    try
                    {
                        mail.Subject = email.Subject;
                        mail.Body = email.Message;
                        mail.IsBodyHtml = email.IsBodyHtml;

                        mail.SubjectEncoding = Encoding.UTF8;
                        email.Status = Status.Completed;

                        mail.ReplyToList.Add(sender);

                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                    catch (SmtpException ex)
                    {
                        email.Status = Status.Failed;
                        Log.Warning(@"Could not send email to {To}. {Ex}. ", email.SentTo, ex);
                    }
                    finally
                    {
                        foreach (IDisposable item in mail.Attachments)
                        {
                            item?.Dispose();
                        }

                        if (deleteAttachmentes)
                        {
                            DeleteFiles(attachments);
                        }
                    }
                }
            }

            return false;
        }

        private void DeleteFiles(params string[] files)
        {
            foreach (string file in files.Where(file => !string.IsNullOrWhiteSpace(file)).Where(File.Exists))
            {
                File.Delete(file);
            }
        }
    }
}