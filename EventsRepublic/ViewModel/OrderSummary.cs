using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.ViewModel
{
    public class OrderSummary
    {
        public string UserId { get; set; } 
        public int Id { get; set; }
        public DateTime Expiryts { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Reserveditems { get; set; }
        public decimal Amount { get; set; }
        public bool AllItemsReserved { get; set; }    
    }
}
