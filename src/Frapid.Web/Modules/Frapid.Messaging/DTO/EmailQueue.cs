using System;
using Frapid.DataAccess;

namespace Frapid.Messaging.DTO
{
    public sealed class EmailQueue : IPoco
    {
        public long QueueId { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
        public string Attachments { get; set; }
        public string Message { get; set; }
        public DateTime AddedOn { get; set; }
        public bool Delivered { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public bool Canceled { get; set; }
    }
}