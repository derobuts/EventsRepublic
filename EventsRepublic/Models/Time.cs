using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    //DR STRANGE
    public static class Time
    {
        //get point in time UTC
        public static Instant GetCurrentInstant()
        {
            return SystemClock.Instance.GetCurrentInstant();
        }
        //convert to local time
        public static LocalDateTime GetLocalTime(DateTime utc, string timezones)
        {
            //parse to an instant
            var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc));
            //timezone to convert to
            var timezone = DateTimeZoneProviders.Tzdb[timezones];
            //instant to zoned local time
            var zoneddatetime = instant.InZone(timezone).LocalDateTime;
            return zoneddatetime;
        }
        //Get current UTC 
        public static DateTime UTC()
        {
            return SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc();
        }
    }
}
