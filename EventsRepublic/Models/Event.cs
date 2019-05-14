using EventsRepublic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventsRepublic.Repository.EventRespositoryv2;

namespace EventsRepublic.Models
{
    public class Event
    {
        public int user_id;
        public string name;
        public int category;
        public int visibility;
        public DateTime startdate;
        public DateTime enddate;
        public int venueid;
        public bool venueispresent;
        public bool hasperformers;
        public List<Recurringpattern> recurringpatterns;
        public int recurring; 
        public Venue venue;
        public TicketClass[] ticketClasses;
        public List<Performer>eventperformers;
        public string photo;
        public string description;

        public class Recurringpattern
        {
            public string recurringstring { get; set; }
            public int intervallengthmins { get; set; }
            public string Endtime { get; set; } 
        }
    }  
}
