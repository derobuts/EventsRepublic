using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class EventSearch
    {
        public string Name { get; set; }
        public int Event_id { get; set; }
        public int maxeventid { get; set; }
        public string Type { get; set; } = "event";
        public string url { get; set; } = "/event/Jayz/";
    }
}
