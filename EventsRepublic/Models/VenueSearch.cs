using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class VenueSearch
    {
        public string Name { get; set; }
        public int Venue_id { get; set; }
        public int maxvenueid { get; set; }
        public string Type { get; set; } = "venue";
    }
}
