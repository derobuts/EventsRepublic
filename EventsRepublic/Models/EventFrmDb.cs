using EventsRepublic.Models.Mpesa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class EventSubinfo
    {
        public Guid Eventid { get; set; }
        public string Name { get; set; }
        public DateTime Startdate { get; set; }
        public int Recurring { get; set; }
        public string photo { get; set; }
        public string PlaceAddress { get; set; }
        public string timezone { get; set; }
        public string Category { get; set; }                  
    }
}
