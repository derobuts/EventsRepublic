using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class BusinessToBusiness 
    {
        private static string url { get; set; } = " https://sandbox.safaricom.co.ke/mpesa/b2b/v1/paymentrequest";
        string securitycredential = "==";
        public void MakePayment(OrderToPay order)
        {
           
        }
    }
}
