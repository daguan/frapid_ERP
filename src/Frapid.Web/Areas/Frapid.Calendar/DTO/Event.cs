using System;
using Frapid.Mapper.Decorators;

namespace Frapid.Calendar.DTO
{
    [TableName("calendar.events")]
    [PrimaryKey("event_id", true, false)]
    public sealed class Event
    {
        public Guid EventId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string GeoLocation { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime? EndsOn { get; set; }
        public bool AllDay { get; set; }
        public string Recurrence { get; set; }
        public string Alarm { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? AuditUserId { get; set; }
        public DateTimeOffset? AuditTs { get; set; }
    }
}