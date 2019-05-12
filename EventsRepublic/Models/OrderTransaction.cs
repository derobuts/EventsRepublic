using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class Order
    {
        int event_id;
        int order_id;
        int user_id;
        decimal amount;
        int status;
        int mobile_refno;
        int payment_method;
        int delivery_status;
        int No_Tickets;
        string Transaction_Code;
        int sales_channel;
        DateTime created;
        DateTime expiry;

        public int Event_id { get => event_id; set => event_id = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public int Status { get => status; set => status = value; }
        public int Mpesa_refno { get => mobile_refno; set => mobile_refno = value; }
        public int Payment_method { get => payment_method; set => payment_method = value; }
        public int Delivery_status { get => delivery_status; set => delivery_status = value; }
        public DateTime Created { get => created; set => created = value; }
        public DateTime Expiry { get => expiry; set => expiry = value; }
        public int OrdersId { get => order_id; set => order_id = value; }
    }
}
