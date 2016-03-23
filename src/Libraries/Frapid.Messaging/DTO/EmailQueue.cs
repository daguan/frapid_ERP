using System;
using Frapid.DataAccess;

namespace Frapid.Messaging.DTO
{
    public sealed class EmailQueue : IPoco
    {
        public long QueueId { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ReplyTo { get; set; }
        public string ReplyToName { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
        public string Attachments { get; set; }
        public string Message { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime SendOn { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public bool Canceled { get; set; }
        public DateTime? CanceledOn { get; set; }
        public bool IsTest { get; set; }
    }
}