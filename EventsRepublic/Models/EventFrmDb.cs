using EventsRepublic.Models.Mpesa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class EventSubinfo
    {
        [JsonProperty("Eventid")]
        public Guid Eventid { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Startdate")]
        public DateTime Startdate { get; set; }
        [JsonProperty("Recurring")]
        public int Recurring { get; set; }
        
        public string photo { get; set; }
        [JsonProperty("PlaceAddress")]
        public string PlaceAddress { get; set; }
        [JsonProperty("timezone")]
        public string timezone { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }                  
    }
}
