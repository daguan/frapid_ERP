using System;
using System.IO;
using SendGrid;

namespace Frapid.SendGridMail
{
    internal static class AttachmentHelper
    {
        internal static SendGridMessage AddAttachments(SendGridMessage message, string[] attachments)
        {
            if (attachments != null)
            {
                foreach (string file in attachments)
                {
                    if (!String.IsNullOrWhiteSpace(file))
                    {
                        if (File.Exists(file))
                        {
                            using (var stream = new FileStream(file, FileMode.Open))
                            {
                                message.AddAttachment(stream, Path.GetFileName(file));
                            }
                        }
                    }
                }
            }

            return message;
        }
    }
}