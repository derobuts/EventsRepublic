using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class EventDetailv3
    {
        public int Eventid { get; set; }
        public string Name { get; set; }
        public string ThumbnailPhoto { get; set; }
        public string LargePhoto { get; set; }
        public string evshowings_id { get; set; }
    }
}
