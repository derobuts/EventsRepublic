using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class CreateOrder
    {
        public int eventid;
        public int ticketclassid;
        public string ticketclassname;
        public int ticketsselected;
        public decimal ticketprice;
        public int ticketdate;
    }
}
