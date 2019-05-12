using EventsRepublic.Models.Mpesa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventsRepublic.Repository.EventRespisotory;

namespace EventsRepublic.Models
{
    public class EventDetails
    {
        public int Eventid { get; set; }
        public string Name { get; set; }
        // public string ThumbnailPhoto { get; set; }
        public string description { get; set; }
        public string LargePhoto { get; set; }
        public string PlaceAddress { get; set; }
        public string timezone { get; set; }
        public IEnumerable<Eventdates> eventDates { get; set; }
    }
}
