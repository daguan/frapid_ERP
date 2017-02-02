using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frapid.Calendar.DAL;
using Frapid.Calendar.ViewModels;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.DataTypes;
using Newtonsoft.Json;
using Event = Frapid.Calendar.DTO.Event;

namespace Frapid.Calendar.Models
{
    public static class CalendarEntry
    {
        public static async Task<Guid> AddEntryAsync(string tenant, CalendarEvent calendarEvent)
        {
            if (calendarEvent == null)
            {
                throw new CalendarEventException("Cannot add a null instance of calendar event.");
            }

            var poco = calendarEvent.ToEvent();
            return await Events.AddEventAsync(tenant, poco).ConfigureAwait(false);
        }

        internal static CalendarEvent Parse(this Event dto)
        {
            var geoLocation = JsonConvert.DeserializeObject<GeographicLocation>(dto.GeoLocation);
            var recurrence = JsonConvert.DeserializeObject<RecurrencePattern>(dto.Recurrence);
            var alarm = JsonConvert.DeserializeObject<Alarm>(dto.Alarm);

            return new CalendarEvent
            {
                EventId = dto.EventId,
                CategoryId = dto.CategoryId,
                UserId = dto.UserId,
                Name = dto.Name,
                Location = dto.Location,
                GeoLocation = geoLocation,
                StartsAt = dto.StartsAt,
                EndsOn = dto.EndsOn,
                AllDay = dto.AllDay,
                Recurrence = recurrence,
                Alarm = alarm,
                Url = dto.Url,
                Note = dto.Note,
                AuditUserId = dto.AuditUserId,
                AuditTs = dto.AuditTs ?? DateTimeOffset.UtcNow
            };
        }

        internal static Event ToEvent(this CalendarEvent calendarEvent)
        {
            return new Event
            {
                EventId = calendarEvent.EventId ?? new Guid(),
                CategoryId = calendarEvent.CategoryId,
                UserId = calendarEvent.UserId ?? 0,
                EndsOn = calendarEvent.EndsOn,
                StartsAt = calendarEvent.StartsAt,
                GeoLocation = JsonConvert.SerializeObject(calendarEvent.GeoLocation),
                Recurrence = JsonConvert.SerializeObject(calendarEvent.Recurrence),
                AllDay = calendarEvent.AllDay ?? false,
                Alarm = JsonConvert.SerializeObject(calendarEvent.Alarm),
                Name = calendarEvent.Name,
                Location = calendarEvent.Location,
                Url = calendarEvent.Url,
                Note = calendarEvent.Note,
                AuditUserId = calendarEvent.AuditUserId,
                AuditTs = calendarEvent.AuditTs ?? DateTimeOffset.UtcNow
            };
        }

        internal static Ical.Net.Event ToIcalEvent(this CalendarEvent calendarEvent)
        {
            var data = new Ical.Net.Event
            {
                DtStart = new CalDateTime(calendarEvent.StartsAt),
                DtEnd = calendarEvent.EndsOn != null ? new CalDateTime(calendarEvent.EndsOn.Value) : null,
                GeographicLocation = calendarEvent.GeoLocation,
                RecurrenceRules = new List<IRecurrencePattern> {calendarEvent.Recurrence},
                IsAllDay = calendarEvent.AllDay ?? false,
                Alarms = {calendarEvent.Alarm},
                Name = calendarEvent.Name,
                Location = calendarEvent.Location,
                Url = new Uri(calendarEvent.Url, UriKind.RelativeOrAbsolute),
                Description = calendarEvent.Note
            };

            return data;
        }
    }
}