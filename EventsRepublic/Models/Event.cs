﻿using EventsRepublic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventsRepublic.Repository.EventRespositoryv2;

namespace EventsRepublic.Models
{
    public class Event
    {

        public string name { get; set; }
        public string category { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int recurring { get; set; }
        public List<Recurringpattern> recurringpatterns;
        public int visibility { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string timezone { get; set; }
        public string description { get; set; }
        public string placeaddress { get; set; }
        public string photo { get; set; }
        public TicketClass[] ticketClasses;
    }
        public class Recurringpattern
        {
            public string recurringstring { get; set; }
            public int intervallengthmins { get; set; }
            public string parsedcron { get; set; }
            public string Endtime { get; set; } 
        }
     
}
