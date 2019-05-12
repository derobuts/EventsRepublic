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
        public Recurringtype recurringtype;
        public bool recurring; 
        public Venue venue;
        public TicketClass[] ticketClasses;
        public List<PerformerEvent> performers;
        public string photo;
        public string description;

        public class Recurringtype
        {
            public string recurringpattern { get; set; }
        }
    }  
}
