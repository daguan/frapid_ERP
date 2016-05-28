using System;

namespace Frapid.Messaging.DTO
{
    public sealed class EmailMessage: IMessage
    {
        public string ReplyToEmail { get; set; }
        public string ReplyToName { get; set; }
        public string Subject { get; set; }
        public Type Type { get; set; }
        public DateTimeOffset EventDateUtc { get; set; }
        public Status Status { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string SentTo { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; } = true;
    }
}