using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class c2bvalidationclass
    {
        public string TransactionType { get; set; }
        public string TransID { get; set; }
        public string TransTime { get; set; }
        public decimal TransAmount { get; set; }
        public int BillRefNumber { get; set; }
        public string ThirdPartyTransID { get; set; }
    }
}
