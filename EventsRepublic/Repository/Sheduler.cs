using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Repository
{
    public class ShedulerRepository
    {
        public IEnumerable<DateTime> GetNextEventOcurrences(string cron)
        {
            var schedule = CrontabSchedule.Parse(cron);
            var s = schedule.GetNextOccurrences(DateTime.Now, DateTime.Now.AddDays(7));
            return s;
        }
    }
}
