using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class EvtTicketClass
    {
    // public int Eventid { get; set; }
     public int TicketId { get; set; }
     public string Name { get; set; }
     public decimal Price { get; set; }
     public bool IsSoldOut { get; set; }
     public int Max_Qt_Per_Order { get; set; }
     public int MinTicketPerOrder { get; set; }
    // public DateTime start_date { get; set; }
    }
}
