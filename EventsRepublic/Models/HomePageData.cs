using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class HomePageData
    {
        public List<EventSubinfo> PopularEvents { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> HappeningThisWeekend { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> Perfomance { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> Sport { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> Theatre { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> Comedy { get; set; } = new List<EventSubinfo>();
        public List<EventSubinfo> Conference { get; set; } = new List<EventSubinfo>();
    }
}
