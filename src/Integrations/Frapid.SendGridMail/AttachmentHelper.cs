using System;
using System.IO;
using System.Web;
using SendGrid.Helpers.Mail;

namespace Frapid.SendGridMail
{
    internal static class AttachmentHelper
    {
        internal static Mail AddAttachments(Mail message, string[] attachments)
        {
            throw new NotImplementedException();
            if(attachments != null)
            {
                foreach(string file in attachments)
                {
                    if(!string.IsNullOrWhiteSpace(file))
                    {
                        if(File.Exists(file))
                        {
                            using(var stream = new FileStream(file, FileMode.Open))
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    string fileName = new FileInfo(file).Name;

                                    var attachment = new Attachment
                                    {
                                        Filename = fileName,
                                        Content = reader.ReadToEnd(),
                                        Type = MimeMapping.GetMimeMapping(file),
                                        Disposition = "attachment",
                                        ContentId = fileName
                                    };

                                    message.AddAttachment(attachment);
                                    //message.AddAttachment(stream, Path.GetFileName(file));
                                }
                            }
                        }
                    }
                }
            }

            return message;
        }
    }
}