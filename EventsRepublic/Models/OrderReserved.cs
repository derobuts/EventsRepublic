using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class OrderReserved
    {
        public int OrdersId { get; set; }
        public DateTime Expiry { get; set; }
        public Guid basket_uuid { get; set; }
        public IEnumerable<ReservedTicket> ReservedTickets;
    }
}
