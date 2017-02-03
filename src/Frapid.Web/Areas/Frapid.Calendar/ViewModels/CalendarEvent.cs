using System;
using System.ComponentModel.DataAnnotations;
using Ical.Net;
using Ical.Net.DataTypes;

namespace Frapid.Calendar.ViewModels
{
    public sealed class CalendarEvent
    {
        public Guid? EventId { get; set; }
        public int CategoryId { get; set; }
        public int? UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public GeographicLocation GeoLocation { get; set; }

        [Required]
        public DateTime StartsAt { get; set; }

        public DateTime? EndsOn { get; set; }

        public bool? AllDay { get; set; }

        public RecurrencePattern Recurrence { get; set; }
        public Alarm Alarm { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? AuditUserId { get; set; }
        public DateTimeOffset? AuditTs { get; set; }
    }
}