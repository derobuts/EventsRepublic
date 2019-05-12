using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class OrderToPay
    {        
        public decimal Amount { get; set; }
        public int Orderid { get; set; }
        public string Msisdn { get; set; }
        public string AccountNo { get; set; }
        public int PaymentMethod { get; set; }
    }
}
